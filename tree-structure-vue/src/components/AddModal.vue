<template>
    <transition name="fade">
        <div class="popup-modal" v-show="open" @click="$emit('close')">
            <div class="window" @click.stop="">
              <h2>Dodawanie węzła</h2>
                <slot></slot>
            </div>
        </div>
    </transition>
</template>
  
  <script>
  export default {
    props: {
      open: {
        type: Boolean,
        default: true,
      },
    },
  };
  </script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.popup-modal {
    background-color: rgba(0, 0, 0, 0.5);
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    padding: 0.5rem;
    display: flex;
    align-items: center;
    z-index: 1;
}

.window {
    background: #fff;
    border-radius: 5px;
    box-shadow: 2px 4px 8px rgba(0, 0, 0, 0.2);
    margin-left: auto;
    margin-right: auto;
    padding: 4rem;
    width: 500px;
}

</style>