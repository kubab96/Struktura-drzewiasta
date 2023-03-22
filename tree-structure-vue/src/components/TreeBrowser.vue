<template>
  <div class="tree-browser">

    <button v-if="isRoot && !toggle" @click="showAll">Rozwiń wszystkie</button>
    <button v-if="isRoot && toggle" @click="hideAll">Zwiń wszystkie</button>
    <button id="addRootBtn" v-if="isRoot" @click="openAddModal(node)">Dodaj korzeń</button>

    <div v-for="node in treeData" :key="node.id" class="nodes">
      <div class="single-node" @click="toggleNode(node)" draggable="true" @dragstart="startDrag($event, node)"
        @drop="onDrop($event, node)" @dragover.prevent @dragenter.prevent>

        <span v-if="node.expanded"><img src="../assets/open-folder.png" alt="edit" width="25" height="25" /></span>
        <span v-if="!node.expanded"><img src="../assets/folder.png" alt="edit" width="25" height="25" /></span>

        <h3>{{ node.name }}</h3>

        <div id="menu" style="margin-left: auto; margin-right: 0">
          <span style="margin-right: 5px" @click.stop="openAddModal(node)"><img id="menuItem" src="../assets/plus.png"
              alt="add" width="25" height="25" />
          </span>
          <span style="margin-right: 5px" @click.stop="openEditModal(node)"><img id="menuItem" src="../assets/pencil.png"
              alt="edit" width="25" height="25" />
          </span>
          <span style="margin-right: 5px" @click.stop="deleteNode(node)"><img id="menuItem" src="../assets/remove.png"
              alt="delete" width="25" height="25" /></span>
        </div>

      </div>


      <tree-browser :tree-data="node.children" :is-root="false" :depth="depth + 1" v-if="node.expanded"
        :style="{ 'margin-left': depth * 20 + 'px' }"></tree-browser>
    </div>

    <EditModal :open="isOpenEdit" @close="isOpenEdit = !isOpenEdit">
      <form @submit.prevent="editNode">
        <div class="form-control" :class="{ invalid: !formEditData.name.isValid }">
          <label for="name">Nazwa</label>
          <input type="text" id="name" v-model.trim="formEditData.name.value" />
          <p v-if="!formEditData.name.isValid">Pole nazwa nie może być puste oraz nie może zawierać więcej niż 25 znaków.</p>
        </div>
        <button @click="editNode">Edytuj</button>
        <button @click.prevent="isOpenEdit = !isOpenEdit">Anuluj</button>
      </form>
    </EditModal>

    <AddModal :open="isOpenAdd" @close="isOpenAdd = !isOpenAdd">
      <form @submit.prevent="addNode">
        <div class="form-control" :class="{ invalid: !formAddData.name.isValid }">
          <label for="name">Nazwa</label>
          <input type="text" id="name" v-model.trim="formAddData.name.value" />
          <p v-if="!formAddData.name.isValid">Pole nazwa nie może być puste oraz nie może zawierać więcej niż 25 znaków.</p>
        </div>
        <button @click.prevent="addNode">Dodaj</button>
        <button @click.prevent="isOpenAdd = !isOpenAdd">Anuluj</button>
      </form>
    </AddModal>

  </div>
</template>

<script>
import EditModal from "./EditModal.vue";
import AddModal from "./AddModal.vue";

export default {
  name: "TreeBrowser",
  props: {
    treeData: {
      type: Array,
      required: true,
    },
    isRoot: {
      type: Boolean,
      default: true,
    },
    depth: {
      type: Number,
      default: 1,
    },
  },
  components: {
    EditModal,
    AddModal,
  },
  data() {
    return {
      toggle: false,
      isOpenEdit: false,
      isOpenAdd: false,
      formEditData: {
        name: {
          value: this.treeData.name,
          isValid: true,
        },
        nodeId: this.treeData.id,
      },
      formAddData: {
        name: {
          value: "",
          isValid: true,
        },
        parentId: this.treeData.id,
      },
      formEditisValid: true,
      formAddisValid: true,
    };
  },
  methods: {
    // Drag and drop with move node functionality
    startDrag(evt, node) {
      evt.dataTransfer.setData("nodeID", node.id);
    },
    onDrop(evt, parentNode) {
      const itemID = evt.dataTransfer.getData("nodeID");
      const formMoveData = {
        id: itemID,
        newParentId: parentNode.id,
      };
      this.$store.dispatch("nodes/moveNode", formMoveData);
    },

    // Show/hide nodes
    toggleNode(node) {
      node.expanded = !node.expanded;
    },
    showAll() {
      this.showNodeAndChildren(this.treeData);
      this.toggle = true;
    },
    hideAll() {
      this.hideNodeAndChildren(this.treeData);
      this.toggle = false;
    },
    showNodeAndChildren(nodes) {
      nodes.forEach((node) => {
        node.expanded = true;
        if (node.children && node.children.length > 0) {
          this.showNodeAndChildren(node.children);
        }
      });
    },
    hideNodeAndChildren(nodes) {
      nodes.forEach((node) => {
        node.expanded = false;
        if (node.children && node.children.length > 0) {
          this.hideNodeAndChildren(node.children);
        }
      });
    },

    // Delete node
    deleteNode(node) {
      if (confirm('Czy na pewno chcesz usunąć ten element?')) {
        this.$store.dispatch("nodes/deleteNode", node.id);
      }
    },

    // Edit node modal
    openEditModal(node) {
      this.formEditData.name.value = node.name;
      this.formEditData.nodeId = node.id;
      this.isOpenEdit = true;
    },
    clearEditValidity(input) {
      this.formEditData[input].isValid = true;
    },
    validateEditForm() {
      this.formEditisValid = true;
      if (this.formEditData.name.value === "" || this.formEditData.name.value.length > 25) {
        this.formEditData.name.isValid = false;
        this.formEditisValid = false;
      }
    },
    editNode() {
      this.validateEditForm();
      if (!this.formEditisValid) {
        return;
      }
      this.$store.dispatch("nodes/editNode", this.formEditData);
    },

    // Add node modal
    openAddModal(node) {
      if(node != null){
        this.formAddData.parentId = node.id;
      }
      this.isOpenAdd = true;
    },
    clearAddValidity(input) {
      this.formAddData[input].isValid = true;
    },
    validateAddForm() {
      this.formAddisValid = true;
      if (this.formAddData.name.value === "" || this.formAddData.name.value.length > 25) {
        this.formAddData.name.isValid = false;
        this.formAddisValid = false;
      }
    },
    addNode() {
      this.validateAddForm();
      if (!this.formAddisValid) {
        return;
      }
        this.$store.dispatch("nodes/addNode", this.formAddData);
    },
  },
};
</script>

<style scoped>
.tree-browser {
  font-family: Arial, sans-serif;
  font-size: 14px;
  font-weight: 300;
  color: #000000;
}

.single-node {
  background-color: rgba(174, 209, 250, 0.5);
  border-radius: 7px;
  padding: 13px;
  justify-content: left;
  margin: 5px;
  display: flex;
  height: 25px;
}

.single-node:hover {
  background-color: rgba(110, 175, 248, 0.5);
}

.cancelBtn {
  font-size: 14px;
  color: #fff;
  background-color: #c4c4c4;
  border: none;
  border-radius: 3px;
  padding: 5px 10px;
  cursor: pointer;
  margin-bottom: 10px;
}

#addRootBtn {
  float: right;
  background-color: #009b22;
}

#addRootBtn:hover {
  background-color: #408d51;
}

.menu {
  justify-content: right;
}

  .tree-browser button {
    font-size: 14px;
    color: #fff;
    width: 130px;
    background-color: #17202b;
    border: none;
    border-radius: 3px;
    padding: 8px;
    cursor: pointer;
    margin-right: 10px;
    margin-bottom: 20px;
  }

  .tree-browser button:hover {
    background-color: #364f6b;
  }

.invalid label {
  color: red;
}

.invalid input,
.invalid textarea {
  border: 1px solid red;
}

.form-control {
  margin: 0.5rem 0;
}

label {
  font-weight: bold;
  display: block;
  margin-bottom: 0.5rem;
}

input[type="checkbox"]+label {
  font-weight: normal;
  display: inline;
  margin: 0 0 0 0.5rem;
}

input,
textarea {
  display: block;
  width: 100%;
  border: 1px solid #ccc;
  font: inherit;
}

input:focus,
textarea:focus {
  background-color: #e6e7fd;
  outline: none;
  border-color: #00218d;
}

input[type="checkbox"] {
  display: inline;
  width: auto;
  border: none;
}

input[type="checkbox"]:focus {
  outline: #002d8d solid 1px;
}

h3 {
  margin: 0.5rem 0;
  margin-left: 8px;
  font-size: 1rem;
}

#menuItem:hover {
  padding-bottom: 2px;
  border-bottom: 2px solid #4d5eff;
}


.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
