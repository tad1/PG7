<script setup lang="ts">
import PaginationComponent from '@/components/PaginationComponent.vue'
import StopInfo from '../components/StopInfo.vue'
import { ref, inject, onMounted, watch } from 'vue'
const stops = ref([]) 
const axios = inject('axios')

const page = ref(1)
const totalPages = ref(1)
const panels = ref([])

const getData = async() => {
  try {
    const data = await axios.get('/stops', {
      params: {
        page: page.value
      }
    })
    console.log(data.data.stops)
    stops.value = data.data.stops
    totalPages.value = data.data.totalPages
  } catch (error) {
    throw new Error(error)
  }
}

onMounted(() => {
  getData()
})

watch(page, () => {
  getData()
  panels.value = []
})

</script>

<template>
  <main>
    <VExpansionPanels v-model="panels" multiple>
        <Suspense>
        <StopInfo v-for="(stop, idx) in stops" :isSaved="false" :stopId="stop.stopId" :key="stop.stopId"/>
      </Suspense>
      </VExpansionPanels>
    <PaginationComponent
      :currentPage="page"
      :totalPages="totalPages"
      @update:currentPage="(newPage: number) => (page = newPage)"
    />
</main>
</template>
