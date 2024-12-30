<script setup lang="ts">
    import { inject, ref } from 'vue'
import BusInfo from './BusInfo.vue';
import DelayEntry from './DelayEntry.vue';
import { useResult, useSnackbarAsync } from '@/composables/useSnackbar';
    const axios = inject('axios')
    const props = defineProps<{
        stopId: number
        isSaved: boolean
    }>()

    const { data } = await axios.get(`/stops/${props.stopId}`)
    const loading = ref(false);
    
    const delayData = ref([]);
    const snackbarPromise = ref<Promise<string>>(Promise.resolve(''));
    const { data:snackbarText, visible:snackbarVisible, error:snackbarError, errorVisible:snackbarErrorVisible} = useSnackbarAsync(snackbarPromise)

    const fetchDelays = async () => {
        console.log("fetching delays");
        if(loading.value) return;
        loading.value = true;

        snackbarPromise.value = axios.get(`/delays/${props.stopId}`).then((res) => {
            delayData.value = res.data.delay;
            return ""
        }).finally(() => loading.value = false);
    }
    const isLogged = localStorage.getItem('token') ? true : false;

    const addStop = async (e: any) => {
        e.preventDefault();
        e.stopPropagation();
        
        snackbarPromise.value = axios.post(`/saved/add/${props.stopId}`).then(() => "Stop added to favorites");
    }

    const removeStop = async (e: any) => {
        e.preventDefault();
        e.stopPropagation();

        snackbarPromise.value = axios.delete(`/saved/remove`, {
            data: {
                stopId: props.stopId
            }
        }).then(() => "Stop removed from favorites");
    }
</script>


<template>
    <Suspense>
        <VExpansionPanel>
            <VExpansionPanelTitle class="stop-info p-4 bg-white shadow-md rounded-lg" @click="fetchDelays">
                <span class="block text-lg font-semibold text-gray-800">{{ data.stopName }}</span>
                <span class="block text-sm text-sky-500 ml-2">{{ data.subName }}</span>
                <button v-if="isLogged && !isSaved" class="block text-sm text-white ml-2 font-semibold border-r-8 cursor-crosshair bg-blue-600 rounded-full w-4 h-4 z-50" @click="addStop">+</button>
                <button v-if="isLogged && isSaved" class="block text-sm text-white ml-2 font-semibold border-r-8 cursor-crosshair bg-red-600 rounded-full w-4 h-4 z-50" @click="removeStop">-</button>
                <v-snackbar v-model="snackbarVisible" color="success" top>{{snackbarText}}</v-snackbar>
                <v-snackbar v-model="snackbarErrorVisible" color="error" top>{{snackbarError}}</v-snackbar>
            </VExpansionPanelTitle>
            <VExpansionPanelText>
                    <span v-if="loading" class="block text-center text-gray-800">Loading...</span>
                    <DelayEntry v-else-if="delayData && (delayData.length > 0)" v-for="delay in delayData" v-bind:data="delay" :key="delay.id"></DelayEntry>
                    <span v-else class="block text-center text-gray-800">No delays</span>
            </VExpansionPanelText>
        </VExpansionPanel>
    </Suspense>
</template>