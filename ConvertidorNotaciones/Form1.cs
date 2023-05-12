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
			} else
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
			for (int i = lista.Length - 1; i >= 0; i--)
			{
				if (lista[i] == " ")
				{

				} else
				// si es un signo de exponente o raiz cuadrada P2
				if (EsIgual(lista[i], "^", "**", "\u221A"))
				{
					if (lista[i] == "**")
					{
						pila.Push('.');
					} else
						pila.Push(Convert.ToChar(lista[i]));
				}
				else
				if (EsIgualSigno(lista[i]) || lista[i] == "(" || lista[i] == ")")
				{
					char letra = Convert.ToChar(lista[i]);
					//Si es parentesis P1
					if (EsIgual(letra, '(', ')'))
					{
						if (letra == ')')
						{
							inicios++;
						} else
						{
							cierres++;
						}
						pila.Push(Convert.ToChar(lista[i]));
						if (inicios > 0 && cierres > 0)
						{
							char p;
							while (pila.Peek() != ')')
							{
								if (pila.Peek() == '(')
								{
									p = pila.Pop();//Saca el (
								}
								else
								{
									p = pila.Pop(); // saca el elemento
									salida += p + " ";
								}

							}
							pila.Pop();//Saca el )

							inicios--; cierres--;
						}
					}
					else
					// si es mutiplicacion o division P3
					if (EsIgual(letra, '*', '/'))
					{
						if (pila.Count > 0 && EsIgual(pila.Peek(), '.', '^', '\u221A'))
						{
							salida += pila.Pop() + " ";
							pila.Push(letra);
						}
						else
						{
							pila.Push(letra);
						}
					} else
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
				} else
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
				if (i == 0)
				{
					while (pila.Count > 0)
					{
						char x = pila.Pop();
						if (x == '(' || x == ')')
						{

						} else
						{
							salida += x + " ";
						}

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
							char p;
							while (pila.Peek() != '(')
							{
								if (pila.Peek() == ')')
								{
									p = pila.Pop();//Saca el )
								}
								else
								{
									p = pila.Pop(); // saca el elemento
									salida += p + " ";
								}

							}
							pila.Pop();//Saca el (

							inicios--; cierres--;
						}
					}
					else
					// si es mutiplicacion o division P3
					if (EsIgual(letra, '*', '/'))
					{
						if (pila.Count > 0 && EsIgual(pila.Peek(), '.', '^', '\u221A'))
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
				if (i == lista.Length - 1)
				{
					while (pila.Count > 0)
					{
						char x = pila.Pop();
						if (x == '(' || x == ')')
						{

						}
						else
						{
							salida += x + " ";
						}
					}

				}
			}
			txtPosfija.Text = (salida);
		}

		private void btnSalir_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
		private bool EsIgualSigno(params string[] coleccion)
		{
			foreach (string signo in coleccion)
			{
				if (signo == '^' + "" || signo == '/' + "" || signo == '*' + "" || signo == '-' + "" || signo == '+' + "" || signo == '=' + "")
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
				if (signo == '^' || signo == '/' || signo == '*' || signo == '-' || signo == '+' || signo == '=')
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
				if (EsIgualSigno(carac))
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
				case 2:
					txtInfija.Text = "( A * B * D ) * C ";
					x++;
					break;
				case 3:
					txtInfija.Text = "( A * B + D ) * C ";
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
		//cambiar tamaño de la forma cuando crece o reduce su tamaño, los controles cambian
		private void Form1_Resize(object sender, EventArgs e)
		{
			int altura = this.Size.Height;
			int anchura = this.Size.Width;
			if (tamañoOriginal.Height == altura && tamañoOriginal.Width == anchura)
			{
				//Textbox
				txtInfija.Size = tamañoOriginaltxtInfija;
				txtInfija.Location = posicionOriginaltxtInfija;
				txtPosfija.Size = tamañoOriginaltxtPosfija;
				txtPosfija.Location = posicionOriginaltxtPosfija;
				txtPrefija.Size = tamañoOriginaltxtPrefija;
				txtPrefija.Location = posicionOriginaltxtPrefija;

				//Botones
				btnConvertir.Size = tamañoOriginalbtnConvertir;
				btnConvertir.Location = posicionOriginalbtnConvertir;
				btnSuma.Size = tamañoOriginalbtnSuma;
				btnSuma.Location = posicionOriginalbtnSuma;
				btnResta.Size = tamañoOriginalbtnResta;
				btnResta.Location = posicionOriginalbtnResta;
				btnMulti.Size = tamañoOriginalbtnMulti;
				btnMulti.Location = posicionOriginalbtnMulti;
				btnDiv.Size = tamañoOriginalbtnDiv;
				btnDiv.Location = posicionOriginalbtnDiv;
				btnDExp.Size = tamañoOriginalbtnDExp;
				btnDExp.Location = posicionOriginalbtnDExp;
				btnExp.Size = tamañoOriginalbtnExp;
				btnExp.Location = posicionOriginalbtnExp;
				btnRaiz.Size = tamañoOriginalbtnRaiz;
				btnRaiz.Location = posicionOriginalbtnRaiz;
				btnInicio.Size = tamañoOriginalbtnInicio;
				btnInicio.Location = posicionOriginalbtnInicio;
				btnCierre.Size = tamañoOriginalbtnCierre;
				btnCierre.Location = posicionOriginalbtnCierre;

				btnMistery.Size = tamañoOriginalbtnMistery;
				btnMistery.Location = posicionOriginalbtnMistery;
				btnAjustar.Size = tamañoOriginalbtnAjustar;
				btnLimpiar.Size = tamañoOriginalbtnLimpiar;
				btnSalir.Size = tamañoOriginalbtnSalir;
			}
			else if(anchura > 50 && anchura < 90)
			{
				//Cambiar las posiciones
			}

		}

		static Size tamañoOriginal;
		static Size tamañoMinimoVentana = new Size(136, 39); 

		static Size tamañoOriginaltxtInfija;
		static Size tamañoOriginaltxtPosfija;
		static Size tamañoOriginaltxtPrefija;
		static Size tamañoOriginalbtnConvertir;
		static Size tamañoOriginalbtnSuma;
		static Size tamañoOriginalbtnResta;
		static Size tamañoOriginalbtnMulti;
		static Size tamañoOriginalbtnDiv;
		static Size tamañoOriginalbtnDExp;
		static Size tamañoOriginalbtnExp;
		static Size tamañoOriginalbtnRaiz;
		static Size tamañoOriginalbtnInicio;
		static Size tamañoOriginalbtnCierre;

		static Size tamañoOriginalbtnMistery;
		static Size tamañoOriginalbtnAjustar;
		static Size tamañoOriginalbtnLimpiar;
		static Size tamañoOriginalbtnSalir;

		static Point posicionOriginaltxtInfija;
		static Point posicionOriginaltxtPosfija;
		static Point posicionOriginaltxtPrefija;
		static Point posicionOriginalbtnConvertir;
		static Point posicionOriginalbtnSuma;
		static Point posicionOriginalbtnResta;
		static Point posicionOriginalbtnMulti;
		static Point posicionOriginalbtnDiv;
		static Point posicionOriginalbtnDExp;
		static Point posicionOriginalbtnExp;
		static Point posicionOriginalbtnRaiz;
		static Point posicionOriginalbtnInicio;
		static Point posicionOriginalbtnCierre;

		static Point posicionOriginalbtnMistery;
		static Point posicionOriginalbtnAjustar;
		static Point posicionOriginalbtnLimpiar;
		static Point posicionOriginalbtnSalir;

		//Necesitas la extension (nugget) System.ValueTuple
		private List<(Control control, Point originalLocation, Size originalSize)> _originalPositions;

		private void Form1_Load(object sender, EventArgs e)
		{
			tamañoOriginal = this.Size;
			tamañoOriginaltxtInfija = txtInfija.Size;
			tamañoOriginaltxtPosfija = txtPosfija.Size;
			tamañoOriginaltxtPrefija = txtPrefija.Size;
			tamañoOriginalbtnConvertir = btnConvertir.Size;
			tamañoOriginalbtnSuma = btnSuma.Size;
			tamañoOriginalbtnResta = btnResta.Size; ;
			tamañoOriginalbtnMulti = btnMulti.Size;
			tamañoOriginalbtnDiv = btnDiv.Size;
			tamañoOriginalbtnDExp = btnDExp.Size;
			tamañoOriginalbtnExp = btnExp.Size;
			tamañoOriginalbtnRaiz = btnRaiz.Size;
			tamañoOriginalbtnInicio = btnInicio.Size;
			tamañoOriginalbtnCierre = btnCierre.Size;

			tamañoOriginalbtnMistery = btnMistery.Size;
			tamañoOriginalbtnAjustar = btnAjustar.Size;
			tamañoOriginalbtnLimpiar = btnLimpiar.Size;
			tamañoOriginalbtnSalir = btnSalir.Size;

			posicionOriginaltxtInfija = txtInfija.Location;
			posicionOriginaltxtPosfija = txtPosfija.Location;
			posicionOriginaltxtPrefija = txtPrefija.Location;
			posicionOriginalbtnConvertir = btnConvertir.Location;
			posicionOriginalbtnSuma = btnSuma.Location;
			posicionOriginalbtnResta = btnResta.Location;
			posicionOriginalbtnMulti = btnMulti.Location;
			posicionOriginalbtnDiv = btnDiv.Location;
			posicionOriginalbtnDExp = btnDExp.Location;
			posicionOriginalbtnExp = btnExp.Location;
			posicionOriginalbtnRaiz = btnRaiz.Location;
			posicionOriginalbtnInicio = btnInicio.Location;
			posicionOriginalbtnCierre = btnCierre.Location;

			posicionOriginalbtnMistery = btnMistery.Location;
			posicionOriginalbtnAjustar = btnAjustar.Location;
			posicionOriginalbtnLimpiar = btnLimpiar.Location;
			posicionOriginalbtnSalir = btnSalir.Location;
			// Guardar las posiciones y tamaños originales de los controles
			_originalPositions = new List<(Control control, Point originalLocation, Size originalSize)>
			{
				(txtInfija, txtInfija.Location, txtInfija.Size),
				(txtPosfija, txtPosfija.Location, txtPosfija.Size),
				(txtPrefija, txtPrefija.Location, txtPrefija.Size),
				(btnConvertir, btnConvertir.Location, btnConvertir.Size),
				(btnSuma, btnSuma.Location, btnSuma.Size),
				(btnResta, btnResta.Location, btnResta.Size),
				(btnMulti, btnMulti.Location, btnMulti.Size),
				(btnDiv, btnDiv.Location, btnDiv.Size),
				(btnDExp, btnDExp.Location, btnDExp.Size),
				(btnExp, btnExp.Location, btnExp.Size),
				(btnRaiz, btnRaiz.Location, btnRaiz.Size),
				(btnInicio, btnInicio.Location, btnInicio.Size),
				(btnCierre, btnCierre.Location, btnCierre.Size),
				(btnMistery, btnMistery.Location, btnMistery.Size),
				(btnAjustar, btnAjustar.Location, btnAjustar.Size),
				(btnLimpiar, btnLimpiar.Location, btnLimpiar.Size),
				(btnSalir, btnSalir.Location, btnSalir.Size)
			};

			// Ajustar la posición y el tamaño de los controles en la primera ejecución
			ResizeControls();
		}
		//Cuando cambie el tamaño de la ventana comprobara esto
		private void ResizeControls()
		{
			int windowAltura = this.ClientSize.Height;
			int windowAncho = this.ClientSize.Width;

			foreach (var controlInfo in _originalPositions)
			{
				Control control = controlInfo.control;
				Point originalLocation = controlInfo.originalLocation;
				Size originalSize = controlInfo.originalSize;

				// Calcular la nueva posición y el nuevo tamaño del control
				int newX = originalLocation.X * windowAncho / tamañoOriginal.Width;
				int newY = originalLocation.Y * windowAltura / tamañoOriginal.Height;
				int newAltura = originalSize.Width * windowAncho / tamañoOriginal.Width;
				int newAncho = originalSize.Height * windowAltura / tamañoOriginal.Height;

				// Establecer la nueva posición y el nuevo tamaño del control
				control.Location = new Point(newX, newY);
				control.Size = new Size(newAltura, newAncho);
			}
		}


		private void btnAjustar_Click(object sender, EventArgs e)
		{
			this.Size = tamañoOriginal;
		}
	}
}
