import {expect, test} from 'vitest'
import {useSnackbarAsync} from '../useSnackbar'
import { ref } from 'vue';

test("useSnackbarAsync works", async () => {
    const resultValue = "result"
    const promise = ref<Promise<string>>(Promise.resolve(resultValue));
    const {data, visible, error, errorVisible} = useSnackbarAsync(promise);

    // NOTE: this is terrible, should be handled properly without timeout
    await new Promise((resolve) => setTimeout(resolve, 100));
    
    expect(data.value).toBe(resultValue);
    expect(visible.value).toBe(true);
    expect(error.value).toBe('');
    expect(errorVisible.value).toBe(false);
});

test("useSnackbarAsync updates with promise", async () => {
    const firstValue = "result"
    const secondValue = "more result"
    const promise = ref<Promise<string>>(Promise.resolve(firstValue));
    const {data, visible, error, errorVisible} = useSnackbarAsync(promise);


    // NOTE: this is terrible, should be handled properly without timeout
    await new Promise((resolve) => setTimeout(resolve, 100));
    promise.value = Promise.resolve(secondValue);
    await new Promise((resolve) => setTimeout(resolve, 100));
    
    expect(data.value).toBe(secondValue);
    expect(visible.value).toBe(true);
    expect(error.value).toBe('');
    expect(errorVisible.value).toBe(false);
});
test("useSnackbarAsync works with errors", async () => {
    const errorValue = "error"
    const e = new Error(errorValue)
    const promise = ref<Promise<string>>(new Promise(()=> {
        throw e;
    }));
    const {data, visible, error, errorVisible} = useSnackbarAsync(promise);

    // NOTE: this is terrible, should be handled properly without timeout
    await new Promise((resolve) => setTimeout(resolve, 100));

    expect(data.value).toBe('');
    expect(visible.value).toBe(false);
    expect(error.value).toBe(error.value);
    expect(errorVisible.value).toBe(true);
});