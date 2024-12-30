<script setup lang="ts">
import StopInfo from '../components/StopInfo.vue'
import { ref, inject, onMounted } from 'vue'
const stops = ref([]) 
const axios = inject('axios')

const getData = async() => {
  try {
    const data = await axios.get('/saved', {})
    stops.value = data.data
  } catch (error) {
    throw new Error(error)
  }
}

onMounted(() => {
  getData()
})
</script>

<template>
  <main>
    <Suspense>
      <VExpansionPanels multiple>
        <StopInfo v-for="stop in stops" :isSaved="true" :stopId="stop" :key="stop"/>
      </VExpansionPanels>
    </Suspense>
    <span v-if="stops.length === 0">No stops saved</span>
</main>
</template>
