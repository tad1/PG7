<script setup lang="ts">
defineProps<{
    delay: number
    theoreticalTime: string
    headsign: string
}>()
</script>

<template>
    <div class="bus-info">
        <span>{{ headsign }}</span>
        <span>{{ theoreticalTime }}</span>
        <span>{{ delay }}</span>
    </div>
</template>