namespace Sorting.Classes
{
    public class SortingAlgs
    {
        public static void Timsort(List<int> array)
        {
            const int MIN_MERGE = 32;
            int n = array.Count;

            // Insertion sort for small arrays
            for (int i = 0; i < n; i += MIN_MERGE)
            {
                int left = i;
                int right = Math.Min(i + MIN_MERGE - 1, n - 1);
                InsertionSort(array, left, right);
            }

            // Merge sorted chunks
            for (int size = MIN_MERGE; size < n; size = 2 * size)
            {
                for (int left = 0; left < n; left += 2 * size)
                {
                    int mid = left + size - 1;
                    int right = Math.Min(left + 2 * size - 1, n - 1);
                    if (mid < right)
                        Merge(array, left, mid, right);
                }
            }
        }

        static void InsertionSort(List<int> array, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                int key = array[i];
                int j = i - 1;

                while (j >= left && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                }

                array[j + 1] = key;
            }
        }

        static void Merge(List<int> array, int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            int[] leftArray = new int[n1];
            int[] rightArray = new int[n2];

            Array.Copy(array.ToArray(), left, leftArray, 0, n1);
            Array.Copy(array.ToArray(), mid + 1, rightArray, 0, n2);

            int i = 0, j = 0;
            int k = left;

            while (i < n1 && j < n2)
            {
                if (leftArray[i] <= rightArray[j])
                {
                    array[k] = leftArray[i];
                    i++;
                }
                else
                {
                    array[k] = rightArray[j];
                    j++;
                }
                k++;
            }

            while (i < n1)
            {
                array[k] = leftArray[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                array[k] = rightArray[j];
                j++;
                k++;
            }
        }



        //Сортування бульбашкою
        public static void BubbleSort(List<int> array)
        {
            int n = array.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }
        //Сортування виборкою
        public static void SelectionSortArray(List<int> array)
        {
            int n = array.Count - 1;
            for (int i = 0; i < n - 1; i++)
            {
                Console.WriteLine(i);
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                    }
                }
                int temp = array[minIndex];
                array[minIndex] = array[i];
                array[i] = temp;
            }
        }
    }
}
