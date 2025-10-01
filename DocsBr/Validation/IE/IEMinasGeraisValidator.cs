using System;
using System.Linq;
using DocsBr.Utils;

namespace DocsBr.Validation.IE
{
    /// <summary>
    /// Validação da IE de Minas Gerais
    /// </summary>
    /// <remarks>
    /// ROTEIRO DE CRÍTICA DA INSCRIÇÃO ESTADUAL:
    ///   http://www.sintegra.gov.br/Cad_Estados/cad_MG.html
    /// </remarks>
    public class IEMinasGeraisValidator : IIEValidator
    {
        private string inscEstadual;

        public IEMinasGeraisValidator(string inscEstadual)
        {
            this.inscEstadual = new OnlyNumbers(inscEstadual).ToString();
        }

        public bool IsValid()
        {
            return HasValidCheckDigits();
        }

        private bool HasValidCheckDigits()
        {
            string ieSemDigitos = this.inscEstadual.Substring(0, this.inscEstadual.Length - 2);
            string number = String.Concat(ieSemDigitos.Substring(0, 3), "0", ieSemDigitos.Substring(3, ieSemDigitos.Length - 3));

            string primeiroDigito = new DigitoVerificador(number)
                                                        .ComMultiplicadoresDeAte(1, 2)
                                                        .InvertendoMultiplicadores()
                                                        .SomandoAlgarismos()
                                                        .PrimeiraDezenaSuperior()
                                                        .CalculaDigito();

            string segundoDigito = new DigitoVerificador(ieSemDigitos)
                                                        .AddDigito(primeiroDigito)
                                                        .ComMultiplicadoresDeAte(2, 11)
                                                        .Substituindo("0", 10, 11)
                                                        .CalculaDigito();

            return String.Concat(primeiroDigito, segundoDigito) == this.inscEstadual.Substring(this.inscEstadual.Length - 2, 2);
        }
    }
}