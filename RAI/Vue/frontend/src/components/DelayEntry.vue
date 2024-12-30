<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps({
    data: {
        type: Object,
        required: true
    }
});

const delayInMinutes = computed(() => (props.data.delayInSeconds ?? 0) / 60);

const getDelayClass = (delayInMinutes: number) => {
    if (delayInMinutes < 0) return "text-red-500 font-bold";
    if (delayInMinutes > 0) return "text-green-500 font-bold";
    return "text-gray-700 font-medium";
};
</script>

<template>
    <div class="delay-entry grid grid-cols-4 gap-4 p-4 bg-gray-50 border border-gray-200 rounded-lg shadow-sm">
        <span class="font-semibold text-gray-800 route-short-name">
            {{ data.routeShortName }}
        </span>
        <span class="text-gray-600 headsign">
            {{ data.headsign }}
        </span>
        <span class="text-gray-500 theoretical-time">
            {{ data.theoreticalTime }}
        </span>
        <span :class="getDelayClass(delayInMinutes)+' delay'" class="text-right">
            {{ delayInMinutes.toFixed(1) }} min</span>
    </div>
</template>