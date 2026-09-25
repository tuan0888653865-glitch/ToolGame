using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000052 RID: 82
	internal class MUJson
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000F034 File Offset: 0x0000D234
		public static object jsonDecode(string json)
		{
			MUJson.lastDecode = json;
			if (json == null)
			{
				return null;
			}
			char[] json2 = json.ToCharArray();
			int num = 0;
			bool flag = true;
			object result = MUJson.parseValue(json2, ref num, ref flag);
			if (flag)
			{
				MUJson.lastErrorIndex = -1;
				return result;
			}
			MUJson.lastErrorIndex = num;
			return result;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000F070 File Offset: 0x0000D270
		public static string jsonEncode(object json)
		{
			StringBuilder stringBuilder = new StringBuilder(2000);
			if (!MUJson.serializeValue(json, stringBuilder))
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000F099 File Offset: 0x0000D299
		public static bool lastDecodeSuccessful()
		{
			return MUJson.lastErrorIndex == -1;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000F0A3 File Offset: 0x0000D2A3
		public static int getLastErrorIndex()
		{
			return MUJson.lastErrorIndex;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000F0AC File Offset: 0x0000D2AC
		public static string getLastErrorSnippet()
		{
			if (MUJson.lastErrorIndex == -1)
			{
				return "";
			}
			int num = MUJson.lastErrorIndex - 5;
			int num2 = MUJson.lastErrorIndex + 15;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 >= MUJson.lastDecode.Length)
			{
				num2 = MUJson.lastDecode.Length - 1;
			}
			return MUJson.lastDecode.Substring(num, num2 - num + 1);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000F108 File Offset: 0x0000D308
		protected static Hashtable parseObject(char[] json, ref int index)
		{
			Hashtable hashtable = new Hashtable();
			MUJson.nextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				int num = MUJson.lookAhead(json, index);
				if (num == 0)
				{
					return null;
				}
				if (num == 6)
				{
					MUJson.nextToken(json, ref index);
				}
				else
				{
					if (num == 2)
					{
						MUJson.nextToken(json, ref index);
						return hashtable;
					}
					string text = MUJson.parseString(json, ref index);
					if (text == null)
					{
						return null;
					}
					num = MUJson.nextToken(json, ref index);
					if (num != 5)
					{
						return null;
					}
					bool flag2 = true;
					object value = MUJson.parseValue(json, ref index, ref flag2);
					if (!flag2)
					{
						return null;
					}
					hashtable[text] = value;
				}
			}
			return hashtable;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000F190 File Offset: 0x0000D390
		protected static ArrayList parseArray(char[] json, ref int index)
		{
			ArrayList arrayList = new ArrayList();
			MUJson.nextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				int num = MUJson.lookAhead(json, index);
				if (num == 0)
				{
					return null;
				}
				if (num == 6)
				{
					MUJson.nextToken(json, ref index);
				}
				else
				{
					if (num == 4)
					{
						MUJson.nextToken(json, ref index);
						break;
					}
					bool flag2 = true;
					object value = MUJson.parseValue(json, ref index, ref flag2);
					if (!flag2)
					{
						return null;
					}
					arrayList.Add(value);
				}
			}
			return arrayList;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000F1F8 File Offset: 0x0000D3F8
		protected static object parseValue(char[] json, ref int index, ref bool success)
		{
			switch (MUJson.lookAhead(json, index))
			{
			case 1:
				return MUJson.parseObject(json, ref index);
			case 3:
				return MUJson.parseArray(json, ref index);
			case 7:
				return MUJson.parseString(json, ref index);
			case 8:
				return MUJson.parseNumber(json, ref index);
			case 9:
				MUJson.nextToken(json, ref index);
				return bool.Parse("TRUE");
			case 10:
				MUJson.nextToken(json, ref index);
				return bool.Parse("FALSE");
			case 11:
				MUJson.nextToken(json, ref index);
				return null;
			}
			success = false;
			return null;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000F2A8 File Offset: 0x0000D4A8
		protected static string parseString(char[] json, ref int index)
		{
			string text = "";
			MUJson.eatWhitespace(json, ref index);
			int num = index;
			index = num + 1;
			char c = json[num];
			bool flag = false;
			while (!flag && index != json.Length)
			{
				num = index;
				index = num + 1;
				c = json[num];
				if (c == '"')
				{
					flag = true;
					break;
				}
				if (c == '\\')
				{
					if (index == json.Length)
					{
						break;
					}
					num = index;
					index = num + 1;
					c = json[num];
					if (c == '"')
					{
						text += "\"";
					}
					else if (c == '\\')
					{
						text += "\\";
					}
					else if (c == '/')
					{
						text += "/";
					}
					else if (c == 'b')
					{
						text += "\b";
					}
					else if (c == 'f')
					{
						text += "\f";
					}
					else if (c == 'n')
					{
						text += "\n";
					}
					else if (c == 'r')
					{
						text += "\r";
					}
					else if (c == 't')
					{
						text += "\t";
					}
					else if (c == 'u')
					{
						if (json.Length - index < 4)
						{
							break;
						}
						char[] array = new char[4];
						Array.Copy(json, index, array, 0, 4);
						text = text + "&#x" + new string(array) + ";";
						index += 4;
					}
				}
				else
				{
					text += c.ToString();
				}
			}
			if (!flag)
			{
				return null;
			}
			return text;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000F418 File Offset: 0x0000D618
		protected static double parseNumber(char[] json, ref int index)
		{
			MUJson.eatWhitespace(json, ref index);
			int lastIndexOfNumber = MUJson.getLastIndexOfNumber(json, index);
			int num = lastIndexOfNumber - index + 1;
			char[] array = new char[num];
			Array.Copy(json, index, array, 0, num);
			index = lastIndexOfNumber + 1;
			return double.Parse(new string(array));
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000F460 File Offset: 0x0000D660
		protected static int getLastIndexOfNumber(char[] json, int index)
		{
			int num = index;
			while (num < json.Length && "0123456789+-.eE".IndexOf(json[num]) != -1)
			{
				num++;
			}
			return num - 1;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000F48E File Offset: 0x0000D68E
		protected static void eatWhitespace(char[] json, ref int index)
		{
			while (index < json.Length)
			{
				if (" \t\n\r".IndexOf(json[index]) == -1)
				{
					return;
				}
				index++;
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000F4B4 File Offset: 0x0000D6B4
		protected static int lookAhead(char[] json, int index)
		{
			int num = index;
			return MUJson.nextToken(json, ref num);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000F4CC File Offset: 0x0000D6CC
		protected static int nextToken(char[] json, ref int index)
		{
			MUJson.eatWhitespace(json, ref index);
			if (index == json.Length)
			{
				return 0;
			}
			char c = json[index];
			index++;
			char c2 = c;
			switch (c2)
			{
			case '"':
				return 7;
			case '#':
			case '$':
			case '%':
			case '&':
			case '\'':
			case '(':
			case ')':
			case '*':
			case '+':
			case '.':
			case '/':
				break;
			case ',':
				return 6;
			case '-':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return 8;
			case ':':
				return 5;
			default:
				switch (c2)
				{
				case '[':
					return 3;
				case '\\':
					break;
				case ']':
					return 4;
				default:
					if (c2 == '{')
					{
						return 1;
					}
					if (c2 == '}')
					{
						return 2;
					}
					break;
				}
				break;
			}
			index--;
			int num = json.Length - index;
			if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
			{
				index += 5;
				return 10;
			}
			if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
			{
				index += 4;
				return 9;
			}
			if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
			{
				index += 4;
				return 11;
			}
			return 0;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000F647 File Offset: 0x0000D847
		protected static bool serializeObjectOrArray(object objectOrArray, StringBuilder builder)
		{
			if (objectOrArray is Hashtable)
			{
				return MUJson.serializeObject((Hashtable)objectOrArray, builder);
			}
			return objectOrArray is ArrayList && MUJson.serializeArray((ArrayList)objectOrArray, builder);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000F674 File Offset: 0x0000D874
		protected static bool serializeObject(Hashtable anObject, StringBuilder builder)
		{
			builder.Append("{");
			IDictionaryEnumerator enumerator = anObject.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext())
			{
				string aString = enumerator.Key.ToString();
				object value = enumerator.Value;
				if (!flag)
				{
					builder.Append(", ");
				}
				MUJson.serializeString(aString, builder);
				builder.Append(":");
				if (!MUJson.serializeValue(value, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("}");
			return true;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000F6F0 File Offset: 0x0000D8F0
		protected static bool serializeDictionary(Dictionary<string, string> dict, StringBuilder builder)
		{
			builder.Append("{");
			bool flag = true;
			foreach (KeyValuePair<string, string> keyValuePair in dict)
			{
				if (!flag)
				{
					builder.Append(", ");
				}
				MUJson.serializeString(keyValuePair.Key, builder);
				builder.Append(":");
				MUJson.serializeString(keyValuePair.Value, builder);
				flag = false;
			}
			builder.Append("}");
			return true;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000F78C File Offset: 0x0000D98C
		protected static bool serializeArray(ArrayList anArray, StringBuilder builder)
		{
			builder.Append("[");
			bool flag = true;
			for (int i = 0; i < anArray.Count; i++)
			{
				object value = anArray[i];
				if (!flag)
				{
					builder.Append(", ");
				}
				if (!MUJson.serializeValue(value, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("]");
			return true;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000F7E8 File Offset: 0x0000D9E8
		protected static bool serializeValue(object value, StringBuilder builder)
		{
			if (value == null)
			{
				builder.Append("null");
			}
			else if (value.GetType().IsArray)
			{
				MUJson.serializeArray(new ArrayList((ICollection)value), builder);
			}
			else if (value is string)
			{
				MUJson.serializeString((string)value, builder);
			}
			else if (value is char)
			{
				MUJson.serializeString(Convert.ToString((char)value), builder);
			}
			else if (value is Hashtable)
			{
				MUJson.serializeObject((Hashtable)value, builder);
			}
			else if (value is Dictionary<string, string>)
			{
				MUJson.serializeDictionary((Dictionary<string, string>)value, builder);
			}
			else if (value is ArrayList)
			{
				MUJson.serializeArray((ArrayList)value, builder);
			}
			else if (value is bool && (bool)value)
			{
				builder.Append("true");
			}
			else if (value is bool && !(bool)value)
			{
				builder.Append("false");
			}
			else
			{
				if (!value.GetType().IsPrimitive)
				{
					return false;
				}
				MUJson.serializeNumber(Convert.ToDouble(value), builder);
			}
			return true;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000F904 File Offset: 0x0000DB04
		protected static void serializeString(string aString, StringBuilder builder)
		{
			builder.Append("\"");
			foreach (char c in aString.ToCharArray())
			{
				if (c == '"')
				{
					builder.Append("\\\"");
				}
				else if (c == '\\')
				{
					builder.Append("\\\\");
				}
				else if (c == '\b')
				{
					builder.Append("\\b");
				}
				else if (c == '\f')
				{
					builder.Append("\\f");
				}
				else if (c == '\n')
				{
					builder.Append("\\n");
				}
				else if (c == '\r')
				{
					builder.Append("\\r");
				}
				else if (c == '\t')
				{
					builder.Append("\\t");
				}
				else
				{
					int num = Convert.ToInt32(c);
					if (num >= 32 && num <= 126)
					{
						builder.Append(c);
					}
					else
					{
						builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
					}
				}
			}
			builder.Append("\"");
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000FA11 File Offset: 0x0000DC11
		protected static void serializeNumber(double number, StringBuilder builder)
		{
			builder.Append(Convert.ToString(number));
		}

		// Token: 0x0400024D RID: 589
		private const int TOKEN_NONE = 0;

		// Token: 0x0400024E RID: 590
		private const int TOKEN_CURLY_OPEN = 1;

		// Token: 0x0400024F RID: 591
		private const int TOKEN_CURLY_CLOSE = 2;

		// Token: 0x04000250 RID: 592
		private const int TOKEN_SQUARED_OPEN = 3;

		// Token: 0x04000251 RID: 593
		private const int TOKEN_SQUARED_CLOSE = 4;

		// Token: 0x04000252 RID: 594
		private const int TOKEN_COLON = 5;

		// Token: 0x04000253 RID: 595
		private const int TOKEN_COMMA = 6;

		// Token: 0x04000254 RID: 596
		private const int TOKEN_STRING = 7;

		// Token: 0x04000255 RID: 597
		private const int TOKEN_NUMBER = 8;

		// Token: 0x04000256 RID: 598
		private const int TOKEN_TRUE = 9;

		// Token: 0x04000257 RID: 599
		private const int TOKEN_FALSE = 10;

		// Token: 0x04000258 RID: 600
		private const int TOKEN_NULL = 11;

		// Token: 0x04000259 RID: 601
		private const int BUILDER_CAPACITY = 2000;

		// Token: 0x0400025A RID: 602
		protected static int lastErrorIndex = -1;

		// Token: 0x0400025B RID: 603
		protected static string lastDecode = "";
	}
}
