namespace Domain.Util
{
    public class ValidarCnpjCPF
    {
        public bool Cnpj(string cnpj)
        {
            cnpj = cnpj.Replace(".", "");
            cnpj = cnpj.Replace("-", "");
            cnpj = cnpj.Replace("/", "");
            cnpj = cnpj.Replace(",", "");

            try
            {
                long.Parse(cnpj);
            }
            catch (NotFiniteNumberException)
            {
                return false;
            }

            // considera-se erro CNPJ's formados por uma sequencia de numeros iguais
            if (cnpj.Equals("00000000000000") || cnpj.Equals("11111111111111")
                || cnpj.Equals("22222222222222") || cnpj.Equals("33333333333333")
                || cnpj.Equals("44444444444444") || cnpj.Equals("55555555555555")
                || cnpj.Equals("66666666666666") || cnpj.Equals("77777777777777")
                || cnpj.Equals("88888888888888") || cnpj.Equals("99999999999999") || (cnpj.Length != 14))
                if (cnpj.Length == 12)
                    cnpj = "00" + cnpj;
                else if (cnpj.Length == 13)
                    cnpj = "0" + cnpj;
                else
                    return (false);

            char dig13, dig14;
            int sm, i, r, num, peso; // "try" - protege o código para eventuais
                                     // erros de conversao de tipo (int)
            try
            {
                // Calculo do 1o. Digito Verificador
                sm = 0;
                peso = 2;
                for (i = 11; i >= 0; i--)
                {
                    // converte o i-ésimo caractere do CNPJ em um número: // por
                    // exemplo, transforma o caractere '0' no inteiro 0 // (48 eh a
                    // posição de '0' na tabela ASCII)
                    num = (int)(cnpj.ElementAt(i) - 48);
                    sm = sm + (num * peso);
                    peso = peso + 1;
                    if (peso == 10)
                        peso = 2;
                }

                r = sm % 11;
                if ((r == 0) || (r == 1))
                    dig13 = '0';
                else
                    dig13 = (char)((11 - r) + 48);

                // Calculo do 2o. Digito Verificador
                sm = 0;
                peso = 2;
                for (i = 12; i >= 0; i--)
                {
                    num = (int)(cnpj.ElementAt(i) - 48);
                    sm = sm + (num * peso);
                    peso = peso + 1;
                    if (peso == 10)
                        peso = 2;
                }
                r = sm % 11;
                if ((r == 0) || (r == 1))
                    dig14 = '0';
                else
                    dig14 = (char)((11 - r) + 48);
                // Verifica se os dígitos calculados conferem com os dígitos
                // informados.
                if ((dig13 == cnpj.ElementAt(12)) && (dig14 == cnpj.ElementAt(13)))
                    return (true);
                else
                    return (false);
            }
            catch (Exception)
            {
                return (false);
            }
        }

        public bool Cpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
            {
                if (cpf.Length == 9)
                {
                    cpf = "00" + cpf;
                }
                else if (cpf.Length == 10)
                    cpf = "0" + cpf;
                else
                    return false;
            }
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public bool IsPis(string pis)
        {
            int[] multiplicador = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            if (pis.Trim().Length != 11)
                return false;
            pis = pis.Trim();
            pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(pis[i].ToString()) * multiplicador[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            return pis.EndsWith(resto.ToString());
        }
    }
}
