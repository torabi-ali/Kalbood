import { ref } from 'vue'
import AppSettings from '@/AppSettings'

export default async function useGetApiFunction<T> (apiUrl: string) {
  const result = ref<T | null>(null)
  const isFailed = ref(false)

  try {
    const response = await fetch(AppSettings.getInstance().baseApiUrl + apiUrl)
    const data = await response.json()
    result.value = data
  } catch (e) {
    isFailed.value = true
  }

  return { result, isFailed }
}
