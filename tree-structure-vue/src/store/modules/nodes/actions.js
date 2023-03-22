import axios from 'axios';

export default{

    addNode(_, formAddData){
        const addNodeData = {
            name: formAddData.name.value,
            parentId: formAddData.parentId,
          };
        axios.post(`https://localhost:7124/api/Tree`, addNodeData)
        .then(response => {if(!response.error){window.location.reload()}})
        .catch(error => console.log(error))
    },

    moveNode(_, formMoveData){
        axios.put(`https://localhost:7124/api/Tree/${formMoveData.id}/move/${formMoveData.newParentId}`)
        .then(response => {if(!response.error){window.location.reload()}})
        .catch(error => console.log(error))
    },

    editNode(_, formEditData){
        const editNodeData = {
            name: formEditData.name.value,
          };
        axios.put(`https://localhost:7124/api/Tree/${formEditData.nodeId}`, editNodeData)
        .then(response => {if(!response.error){window.location.reload()}})
        .catch(error => console.log(error))
    },

    deleteNode(_, nodeId){
        axios.delete(`https://localhost:7124/api/Tree/${nodeId}`)
        .then(response => {if(!response.error){window.location.reload()}})
        .catch(error => console.log(error))
    },
};