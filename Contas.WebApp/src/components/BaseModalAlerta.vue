<template>
  <div>
    <b-modal ref="modalAlerta" hide-footer centered title="Harrison Investimentos" visible>
      <div class="d-block text-center">
        <h3>{{ text }}</h3>
      </div>
      <div class="d-block text-right">
        <b-btn class="mt-3" variant="outline-success" @click="onConfirm">Ok</b-btn>
      </div>
    </b-modal>
  </div>
</template>

<script>
export default {
  props: ['text'],
  data() {
    return {
    };
  },
  methods: {
    onConfirm() {
      this.$emit('confirm');
    },
  },
  created() {
    // this.$refs.modalAlerta.show()
  },
};
</script>
<style>

</style>
