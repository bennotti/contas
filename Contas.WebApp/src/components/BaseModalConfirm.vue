<template>
  <div>
    <b-modal ref="myModalRef" hide-footer title="Using Component Methods">
      <div class="d-block text-center">
        <h3>Hello From My Modal!</h3>
      </div>
      <b-btn class="mt-3" variant="outline-danger" block>Close Me</b-btn>
    </b-modal>
  </div>
</template>

<script>
export default {
  props: ['open', 'user'],
  data() {
    return {
    };
  },
  methods: {
    onConfirm() {
      this.$emit('confirm');
    },
    onCancel() {
      this.$emit('cancel');
    },
  },
  created() {
  },
};
</script>
<style>

</style>
