import { ref, watchEffect, type Ref } from "vue"

export function useSnackbarAsync(promise: Ref<Promise<string>>): { data: Ref<string>, visible: Ref<boolean>, error: Ref<string>, errorVisible: Ref<boolean> } {
    const data = ref<string>('')
    const visible = ref(false)
    const error = ref<string>('')
    const errorVisible = ref(false)

    const resolve = () => {
        promise.value.then((response) => {
            if(response !== '') {
                data.value = response
                visible.value = true
            }
        }).catch((e) => {
            error.value = e
            errorVisible.value = true
            console.error(e)
        })
    }

    watchEffect(() => {
        resolve()
    })

    return { data, visible, error, errorVisible }
}