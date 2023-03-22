<template>
  <div class="card">
    <h2>Widok drzewa</h2>
    <button @click="changeSortDirection">Odwróć kierunek sortowania</button>
    <TreeBrowser :tree-data="treeData"></TreeBrowser>
    <p>Przeciągnij węzeł, aby go przestawić.</p>
  </div>
</template>

<script>
import TreeBrowser from "./components/TreeBrowser.vue";
import axios from "axios";

export default {
  name: "App",
  components: {
    TreeBrowser,
  },
  data() {
    return {
      treeData: [],
      sortDirection: 0
    };
  },
  methods:{
    changeSortDirection(){
      if(this.sortDirection == 0){
        this.sortDirection = 1
      }
      else{
        this.sortDirection = 0
      }
      this.getNodes();
    },
    getNodes(){
      axios
      .get(`https://localhost:7124/api/Tree?SortDirection=${this.sortDirection}`)
      .then((response) => {
        this.treeData = response.data;
      })
      .catch((error) => {
        console.error(error);
      });
    }
  },
  created() {
    this.getNodes()
  },
};
</script>


<style scoped>
.card {
    border-radius: 12px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.26);
    padding: 1rem;
    margin: auto;
    margin-top: 40px;
    max-width: 85rem;
    
  }

  .card button {
  font-size: 14px;
  color: #fff;
  background-color: #17202b;
  border: none;
  border-radius: 3px;
  padding: 8px;
  position:absolute;
  cursor: pointer;
  margin-left: 135px;
}

.card  button:hover {
  background-color: #364f6b;
}
</style>