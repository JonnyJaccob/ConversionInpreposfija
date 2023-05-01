using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertidorNotaciones
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnConvertir_Click(object sender, EventArgs e)
		{
			string texto = txtInfija.Text;
			if (string.IsNullOrEmpty(texto))
			{
				MessageBox.Show("El texto está vacío o es nulo");
			}else
			{
				ConversionPrefija(texto);
				ConversionPosfija(texto);
			}
		}
		private void ConversionPrefija(string texto)
		{
			txtPrefija.Text = "";
			string[] lista = SepararOperadores(texto);
			string salida = "";
			Stack<char> pila = new Stack<char>();
			int inicios = 0;
			int cierres = 0;
			for (int i = lista.Length-1; i >= 0; i--)
			{
				if(lista[i]==" ")
				{

				}else
				// si es un signo de exponente o raiz cuadrada P2
				if(EsIgual(lista[i] , "^","**", "\u221A") )
				{
					if(lista[i]=="**")
					{
						pila.Push('.');
					}else
					pila.Push(Convert.ToChar(lista[i]));
				}
				else
				if(EsIgualSigno(lista[i]) || lista[i] == "(" || lista[i]==")")
				{
					char letra = Convert.ToChar(lista[i]);
					//Si es parentesis P1
					if (EsIgual(letra,'(',')'))
					{
						if(letra == ')')
						{
							inicios++;
						}else
						{
							cierres++;
						}
						pila.Push(Convert.ToChar(lista[i]));
						if(inicios>0 && cierres>0 )
						{
							pila.Pop();//Saca el (
							char c = pila.Pop(); // saca el elemento
							pila.Pop();//Saca el )
							salida += c + " ";
							inicios--;cierres--;
						}
					}
					else 
					// si es mutiplicacion o division P3
					if(EsIgual(letra,'*','/'))
					{
						if(EsIgual(pila.Peek(),'.','^', '\u221A'))
						{
							salida += pila.Pop() + " ";
							pila.Push(letra);
						}
						else
						{
							pila.Push(letra);
						}
					}else
					//si es suma o resta P4
					if(EsIgual(letra, '+', '-'))
					{
						if (pila.Count > 0 && EsIgual(pila.Peek(), '.', '^', '\u221A','*','/'))
						{
							salida += pila.Pop() + " ";
							pila.Push(letra);
						}
						else
						{
							pila.Push(letra);
						}
					}
					else
					//si es suma o resta P5
					if (letra == '=')
					{
						if (pila.Count > 0 && EsIgual(pila.Peek(), '.', '^', '\u221A', '*', '/','+','-'))
						{
							salida += pila.Pop() + " ";
							pila.Push(letra);
						}
						else
						{
							pila.Push(letra);
						}
					}
					else
					{
						//algo fuera de lo comun, no hacer nada
					}
				}else
				{
					string sinEspacios = "";
					foreach (char item in lista[i])
					{
						if(item != ' ')
						{
							sinEspacios += item;
						}
					}
					salida += sinEspacios + " ";
				}
				if(i == 0)
				{
					while (pila.Count > 0)
					{
						salida += pila.Pop() + " ";
					}

				}
			}
			txtPrefija.Text = InvertirElementos(salida);
		}
		private void ConversionPosfija(string texto)
		{
			txtPosfija.Text = "";
			string[] lista = SepararOperadores(texto);
			string salida = "";
			Stack<char> pila = new Stack<char>();
			int inicios = 0;
			int cierres = 0;
			for (int i = 0; i <= lista.Length - 1; i++)
			{
				if (lista[i] == " ")
				{

				}
				else
				// si es un signo de exponente o raiz cuadrada P2
				if (EsIgual(lista[i], "^", "**", "\u221A"))
				{
					if (lista[i] == "**")
					{
						pila.Push('.');
					}
					else
						pila.Push(Convert.ToChar(lista[i]));
				}
				else
				if (EsIgualSigno(lista[i]) || lista[i] == "(" || lista[i] == ")")
				{
					char letra = Convert.ToChar(lista[i]);
					//Si es parentesis P1
					if (EsIgual(letra, '(', ')'))
					{
						if (letra == '(')
						{
							inicios++;
						}
						else
						{
							cierres++;
						}
						pila.Push(Convert.ToChar(lista[i]));
						if (inicios > 0 && cierres > 0)
						{
							pila.Pop();//Saca el (
							char c = pila.Pop(); // saca el elemento
							pila.Pop();//Saca el )
							salida += c + " ";
							inicios--; cierres--;
						}
					}
					else
					// si es mutiplicacion o division P3
					if (EsIgual(letra, '*', '/'))
					{
						if (EsIgual(pila.Peek(), '.', '^', '\u221A'))
						{
							salida += pila.Pop() + " ";
							pila.Push(letra);
						}
						else
						{
							pila.Push(letra);
						}
					}
					else
					//si es suma o resta P4
					if (EsIgual(letra, '+', '-'))
					{
						if (pila.Count > 0 && EsIgual(pila.Peek(), '.', '^', '\u221A', '*', '/'))
						{
							salida += pila.Pop() + " ";
							pila.Push(letra);
						}
						else
						{
							pila.Push(letra);
						}
					}
					else
					//si es suma o resta P5
					if (letra == '=')
					{
						if (pila.Count > 0 && EsIgual(pila.Peek(), '.', '^', '\u221A', '*', '/', '+', '-'))
						{
							salida += pila.Pop() + " ";
							pila.Push(letra);
						}
						else
						{
							pila.Push(letra);
						}
					}
					else
					{
						//algo fuera de lo comun, no hacer nada
					}
				}
				else
				{
					string sinEspacios = "";
					foreach (char item in lista[i])
					{
						if (item != ' ')
						{
							sinEspacios += item;
						}
					}
					salida += sinEspacios + " ";
				}
				if (i == lista.Length-1)
				{
					while (pila.Count > 0)
					{
						salida += pila.Pop() + " ";
					}

				}
			}
			txtPosfija.Text = (salida);
		}

		private void btnSalir_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
		private bool EsIgualSigno(params string[] coleccion )
		{
			foreach (string signo in coleccion)
			{
				if (signo == '^' + "" || signo == '/' + "" || signo == '*'+"" || signo == '-'+"" || signo == '+'+"" || signo == '='+"")
				{
					return true;
				}
			}
			return false;
		}
		private bool EsIgual(string cad1, params string[] conjunto)
		{
			foreach (string cad in conjunto)
			{
				if (cad == cad1)
					return true;
			}
			return false;
		}
		private bool EsIgual(char cad1, params char[] conjunto)
		{
			foreach (char cad in conjunto)
			{
				if (cad == cad1)
					return true;
			}
			return false;
		}
		private bool EsIgualSigno(params char[] coleccion)
		{
			foreach (char signo in coleccion)
			{
				if (signo == '^'  || signo == '/' || signo == '*' || signo == '-' || signo == '+' || signo == '=')
				{
					return true;
				}
			}
			return false;
		}
		private void CrearArreglo(string cadena)
		{
			char[] list = cadena.ToCharArray();
			foreach (char carac in list)
			{
				if(EsIgualSigno(carac))
				{

				}
			}
		}
		private string[] SepararOperadores(string cadena)
		{
			// Expresión regular que busca operadores aritméticos y paréntesis
			string patron = @"([\+\-\*\/\(\)=])";

			// Separamos la cadena en función de los operadores y paréntesis
			string[] partes = Regex.Split(cadena, patron);

			// Filtramos las partes vacías (por ejemplo, entre dos operadores)
			partes = partes.Where(p => !string.IsNullOrEmpty(p)).ToArray();

			// Reemplazamos las partes que representan números por su equivalente en letras
			for (int i = 0; i < partes.Length; i++)
			{
				double numero;
				if (double.TryParse(partes[i], out numero))
				{
					partes[i] = numero.ToString(CultureInfo.InvariantCulture);
				}
			}

			return partes;
		}
		private string InvertirElementos(string expresion)
		{
			string[] elementos = expresion.Split(' ');
			List<string> elementosInvertidos = new List<string>();

			// invertir solo los elementos que no son números
			foreach (string elemento in elementos)
			{
				if (double.TryParse(elemento, out double numero))
				{
					// si el elemento es un número, lo agregamos a la lista tal cual
					elementosInvertidos.Add(elemento);
				}
				else
				{
					// si el elemento no es un número, invertimos sus caracteres y lo agregamos a la lista
					char[] caracteres = elemento.ToCharArray();
					Array.Reverse(caracteres);
					elementosInvertidos.Add(new string(caracteres));
				}
			}

			// invertimos toda la lista de elementos invertidos
			elementosInvertidos.Reverse();

			// unimos los elementos invertidos para formar la expresión invertida
			return string.Join(" ", elementosInvertidos);
		}


		private void btnSuma_Click(object sender, EventArgs e)
		{
			txtInfija.Text += "+";
		}

		private void btnResta_Click(object sender, EventArgs e)
		{
			txtInfija.Text += "-";
		}

		private void btnMulti_Click(object sender, EventArgs e)
		{
			txtInfija.Text += "*";
		}

		private void btnDiv_Click(object sender, EventArgs e)
		{
			txtInfija.Text += "/";
		}

		private void btnDExp_Click(object sender, EventArgs e)
		{
			txtInfija.Text += "**";
		}

		private void btnExp_Click(object sender, EventArgs e)
		{
			txtInfija.Text += "^";
		}

		private void btnRaiz_Click(object sender, EventArgs e)
		{
			txtInfija.Text += "\u221A";
		}

		private void btnInicio_Click(object sender, EventArgs e)
		{
			txtInfija.Text += "(";
		}

		private void btnCierre_Click(object sender, EventArgs e)
		{
			txtInfija.Text += ")";
		}
		int x = 0;
		private void btnMistery_Click(object sender, EventArgs e)
		{
			switch (x)
			{
				case 0:
					txtInfija.Text = "X = ( ( 3 * 4 ) + 2)";
					x++;
					break;
				case 1:
					txtInfija.Text = "X = Y + 10";
					x++;
					break;
				default:
					x = 0;
					break;
			}

		}

		private void btnLimpiar_Click(object sender, EventArgs e)
		{
			txtPosfija.Text = "";
			txtInfija.Text = "";
			txtPrefija.Text = "";
		}
	}
}
