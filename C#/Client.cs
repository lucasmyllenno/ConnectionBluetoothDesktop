using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Windows.Forms;
using InTheHand.Net.Sockets;

namespace ConsoleAdapterBluetooth
{
    class Client
    {
		private Guid UUID;
		private String PIN;
        	private BluetoothAddress bluetoothAddress;
		private BluetoothClient bluetoothClient;
		
		private String stringSend;
		private String status;
		
		public Client()
		{
			UUID = new Guid("00000000-0000-0000-0000-000000000000");
	            	PIN = "1234";
			
			bluetoothAddress = getMacAddress();
		}
		
		public void start(BluetoothDeviceInfo serverToSend)
		{
			try
			{
				if (serverToSend.Authenticated)
				{
					bluetoothClient = new BluetoothClient();
                    			bluetoothClient.Connect(serverToSend.DeviceAddress, UUID);
					Console.WriteLine("Conexão Estabelecida!");

                    			Thread thread = new Thread(new ThreadStart(threadClientBluetooth));
                    			thread.Start();
				}
				else
				{
                    			Console.WriteLine("Servidor Não Autenticado!");
				}
			}
			catch (Exception e)
			{
                		Console.WriteLine("Erro: " + e.ToString());
			}
		}
		
		public void threadClientBluetooth()
		{
			try
			{
				Stream stream = bluetoothClient.GetStream();

		                byte[] sent = Encoding.ASCII.GetBytes(stringSend);
		                stream.Write(sent, 0, sent.Length);
		                stream.Flush();

                		Console.WriteLine("Mensagem Enviada!");
			}
			catch (Exception e)
			{
                		Console.WriteLine("Erro: " + e.ToString());
			}
		}
		
		public BluetoothAddress getMacAddress()
		{
		         BluetoothRadio bluetoothRadio = BluetoothRadio.PrimaryRadio;
		         BluetoothAddress bluetoothAddress = bluetoothRadio.LocalAddress;
		         return bluetoothAddress;
		}
		
		public void close()
        	{
            		bluetoothClient.Close();
            		bluetoothClient.Dispose();
            		Console.WriteLine("Conexão terminada!");
        	}
		
		public void setStringSend(String status)
		{
			this.stringSend = stringSend;
		}
		
		public String getStringSend()
		{
			return stringSend;
		}
	}
}
