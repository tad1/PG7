<script setup lang="ts">
import BusInfo from '../components/BusInfo.vue'
defineProps<{
  buses: {
    id: string;
    delayInSeconds: number;
    estimatedTime: string;
    headsign: string;
    routeId: number;
    tripId: number;
    status: string;
    theoreticalTime: string;
    timestamp: string;
    trip: number;
    vehicleCode: number;
    vehicleId: number;
  }[];
}>();
</script>

<template>
    <div v-for="bus in buses" :key="bus.id">
      <BusInfo :delay="bus.delay" :theoreticalTime="bus.theoreticalTime" :headsign="bus.headsign" />
    </div>
</template>