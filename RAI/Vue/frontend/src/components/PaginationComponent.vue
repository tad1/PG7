<script setup lang="ts">
import { defineProps, defineEmits, computed } from 'vue'

const props = defineProps({
  currentPage: {
    type: Number,
    required: true,
  },
  totalPages: {
    type: Number,
    required: true,
  },
})

const pages = computed(() => {
  let offset = 3
  const rangeSize = Math.min(2*offset+1, props.totalPages)
  offset = Math.floor(rangeSize / 2)
  let lowOffset = Math.min(offset, props.currentPage-1)
  let highOffset = Math.min(offset, props.totalPages - props.currentPage)
  if(lowOffset < offset) {
    highOffset += offset - lowOffset
  }
  else if(highOffset < offset) {
    lowOffset += offset - highOffset
  }
  const start = props.currentPage - lowOffset
  const end = props.currentPage + highOffset
  return Array.from({ length: end - start + 1 }, (_, i) => i + start)
})
const emit = defineEmits(['update:currentPage'])

const goToPage = (page: number) => {
  emit('update:currentPage', page)
}
</script>

<template>
    <nav class="pagination flex justify-center items-center space-x-2 mt-4">
      <button
        class="px-4 py-2 rounded"
        :disabled="currentPage === 1"
        @click="goToPage(currentPage - 1)"
      >
        Prev
      </button>
      <button
        v-for="page in pages"
        :key="page"
        class="px-4 py-2 rounded"
        :class="{
          'bg-blue-500 text-white': page === currentPage,
          '': page !== currentPage,
        }"
        @click="goToPage(page)"
      >
        {{ page }}
      </button>
      <button
        class="px-4 py-2 rounded"
        :disabled="currentPage === totalPages"
        @click="goToPage(currentPage + 1)"
      >
        Next
      </button>
    </nav>
  </template>
  
<style scoped>
  .pagination button:disabled {
    background-color: rgba(140, 140, 140, 0.25);
  }
  .pagination button {
    border: 1px solid rgba(140, 140, 140, 0.25);
  }
  .pagination button:hover {
    background-color: rgba(140, 140, 140, 0.25);
  }
</style>