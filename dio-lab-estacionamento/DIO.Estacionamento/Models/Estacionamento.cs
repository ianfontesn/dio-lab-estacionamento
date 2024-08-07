using DIO.Estacionamento.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DIO.Estacionamento.Models
{
    public class Estacionamento(Dictionary<TipoVeiculo, decimal> precoInicialVeiculos, Dictionary<TipoVeiculo, decimal> precoHoraVeiculos)
    {
        private readonly Dictionary<TipoVeiculo, decimal> PrecoInicialVeiculos = precoInicialVeiculos;
        private readonly Dictionary<TipoVeiculo, decimal> PrecoHoraVeiculos = precoHoraVeiculos;
        private List<Veiculo> VeiculosEstacionados = [];

        //Adiciona um veiculo a lista
        public void AdicionarVeiculo(Veiculo veiculo)
        {
            VeiculosEstacionados = [veiculo]; 
        }

        //Remove o veículo da lista.
        public bool RemoverVeiculo(string placa)
        {
            Veiculo? veiculo = ObterVeiculo(placa);

            return veiculo is not null && VeiculosEstacionados.Remove(veiculo);
        }

        //Lista todos os veículos baseados no tostring da classe veículo.
        public void ListarVeiculos()
        {
            if (VeiculosEstacionados.Count > 0)
            {
                for (int i = 0; i < VeiculosEstacionados.Count; i++)
                {
                    Console.WriteLine($"\n---- {i+1} ----\n{VeiculosEstacionados[i]}");
                }
            }
            else
            {
                Console.WriteLine("Não há veículos estacionados.");
            }
        }

        //Calcula preço da permanencia (Valor inicial + valor por hora, sendo ignorado minutos, considera apenas hora cheia)
        public decimal CalcularPrecoSaidaVeiculo(Veiculo veiculo)
        {
            var totalHoras = Math.Ceiling((decimal)(DateTime.Now - veiculo.HoraEntrada).TotalHours);
            decimal precoFinal = (PrecoHoraVeiculos[veiculo.Tipo] * totalHoras) + PrecoInicialVeiculos[veiculo.Tipo];
            return precoFinal;

        }

        //Busca o veículo pela placa
        public Veiculo? ObterVeiculo(string placa)
        {
            Veiculo? veiculo = VeiculosEstacionados.FirstOrDefault(v => v.Placa == placa);
            return veiculo;
        }

        //Regex verifica string no formato aaa1234 ou aaa1a23
        public static bool VerificarPlacaValida(string placa)
        {
            string pattern = @"(?i)^[A-Z]{3}\d{4}$|^[A-Z]{3}\d[A-Z]\d{2}$";

            return !string.IsNullOrWhiteSpace(placa) && Regex.IsMatch(placa, pattern);
        }
    }
}
