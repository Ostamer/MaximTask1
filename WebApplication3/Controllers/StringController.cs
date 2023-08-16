using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApplication3.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class StringController : ControllerBase
	{
		[HttpGet]
		public IActionResult ProcessString(string inputString)
		{
			char[] chars = inputString.ToCharArray();
			string result = englishLowRegister(chars);
			if (String.IsNullOrEmpty(result))
			{
				string finalString = evenOrOdd(chars);
				string countChars = countAllChars(finalString);
				string subString = maxSubString(finalString);
				char[] sortedArray = finalString.ToCharArray();
				HeapSort(sortedArray);
				char[] truncatedArray = finalString.ToCharArray();
				string truncate = truncateArray(truncatedArray);
				string[] mas = new string[5];
				mas[0] = finalString;
				mas[1] = countChars;
				mas[2] = subString;
				mas[3] = new string(sortedArray);
				mas[4] = truncate;
				return Ok(mas);
			}
			else
			{
				return BadRequest("HTTP ошибка 400 Bad Request. Ошибка((( в строке находятся вот такие недопустимые символы: " + result);
			}

		}

		private static int Partition(char[] A, int l, int r)
		{
			int barrier = A[(l + r) / 2];
			int i = l;
			int j = r;

			while (i <= j)
			{
				while (A[i] < barrier) i++;
				while (A[j] > barrier) j--;
				if (i >= j) break;

				int m = i++;
				int u = j--;

				char tmp = A[m];
				A[m] = A[u];
				A[u] = tmp;
			}

			return j;

		}

		private static void QuickSort(char[] arr, int l, int r)
		{
			if (l < r)
			{
				int q = Partition(arr, l, r);
				QuickSort(arr, l, q);
				QuickSort(arr, q + 1, r);
			}
		}

		private static void HeapSort(char[] arr)
		{
			int n = arr.Length;
			for (int i = n / 2 - 1; i >= 0; i--)
			{
				Heapify(arr, n, i);
			}
			for (int i = n - 1; i >= 0; i--)
			{
				char temp = arr[0];
				arr[0] = arr[i];
				arr[i] = temp;
				Heapify(arr, i, 0);
			}
		}

		private static void Heapify(char[] arr, int n, int i)
		{
			int largest = i;
			int l = 2 * i + 1;
			int r = 2 * i + 2;
			if (l < n && arr[l] > arr[largest])
				largest = l;
			if (r < n && arr[r] > arr[largest])
				largest = r;
			if (largest != i)
			{
				char swap = arr[i];
				arr[i] = arr[largest];
				arr[largest] = swap;
				Heapify(arr, n, largest);
			}
		}

		private static string englishLowRegister(char[] chars)
		{
			string englishAlphabet = "qwertyuiopasdfghjklzxcvbnm";
			string result = "";
			for (int i = 0; i < chars.Length; i++)
			{
				if (!englishAlphabet.Contains(chars[i]))
				{
					result += chars[i];
				}
			}
			return result;
		}

		private static string evenOrOdd(char[] chars)
		{
			int lenString = chars.Length;
			string result = "";
			if (lenString % 2 != 0)
			{
				char[] reverseChar = new char[lenString];
				Array.Copy(chars, 0, reverseChar, 0, lenString);
				Array.Reverse(reverseChar);
				result = new string(reverseChar) + new string(chars);
			}
			else
			{
				for (int i = chars.Length / 2 - 1; i >= 0; i--)
				{
					result += chars[i];
				}
				for (int i = chars.Length - 1; i >= chars.Length / 2; i--)
				{
					result += chars[i];
				}
			}
			return result;
		}

		private static string countAllChars(string inputString)
		{
			char[] chars = inputString.ToCharArray();
			Dictionary<char, int> result = new Dictionary<char, int>();
			foreach (char c in chars)
			{
				if (result.ContainsKey(c))
				{
					result[c]++;
				}
				else
				{
					result[c] = 1;
				}
			}
			string answer = "";
			foreach (var c in result)
			{
				answer+=$"Буква: {c.Key} встречается: {c.Value} раз. ";
			}

			return answer;
		}

		private static string maxSubString(string inputString)
		{
			char[] chars = inputString.ToCharArray();
			string alphabet = "aeoiuy";
			int first = -1;
			int second = -1;
			for (int i = 0; i < chars.Length; i++)
			{
				if (alphabet.Contains(chars[i]))
				{
					if (first == -1)
					{
						first = i;
					}
					else
					{
						second = i;
					}
				}
			}
			if (first == -1 || second == -1)
			{
				return "В данной строке менее двух гласных букв";
			}
			else
			{
				char[] result = new char[second - first + 1];
				Array.Copy(chars, first, result, 0, second - first + 1);
				return new string(result);
			}
		}
		private static int getRandomNumber(int length)
		{
			int result = 0;
			bool a = true;
			try
			{
				length--;
				string randomizer = "https://api.rand.by/v1/integer/?count=1&min=1&max=" + length;
				length++;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(randomizer);
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				StreamReader reader = new StreamReader(response.GetResponseStream());
				string answer = reader.ReadLine();
				int.TryParse(string.Join("", answer.Where(c => char.IsDigit(c))), out result);
			}
			catch
			{
				a = false;
			}
			if (a == false)
			{
				Random rnd = new Random();
				result = rnd.Next(0, length);
			}
			return result;
		}
		private static string truncateArray(char[] chars)
		{
			int randomNumber = getRandomNumber(chars.Length);
			string result = "";
			for (int i = 0; i < chars.Length; i++)
			{
				if (i != randomNumber)
				{
					result += chars[i];
				}
			}
			return result;
		}
	}
}