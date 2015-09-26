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
    class Server
    {
        	private Guid UUID;
		private BluetoothListener bluetoothListener;
	        private BluetoothClient bluetoothClient;
		
		public Server()
		{
			UUID = new Guid("00000000-0000-0000-0000-000000000000");
		}
		
		public void start()
		{
	            	try
	            	{
	                	bluetoothListener = new BluetoothListener(UUID);
	                	bluetoothListener.Start();
	
	                	Console.WriteLine("Aguardando Clientes..");
	
	                	if (bluetoothListener.Pending())
	                	{
	                		Thread thread = new Thread(new ThreadStart(readInfoClient));
	                    		thread.Start();
	                	}
	            	}
	            	catch (Exception e)
	            	{
	                	Console.WriteLine("Erro: " + e.ToString());
	            	}
		}
		
		public void readInfoClient()
		{
            		bluetoothClient = bluetoothListener.AcceptBluetoothClient();
            		Console.WriteLine("Cliente Conectado!");

			Stream stream = bluetoothClient.GetStream();
			
			while (bluetoothClient.Connected)
            		{
                		try
		                {
		                	byte[] byteReceived = new byte[1024];
		                    	int read = stream.Read(byteReceived, 0, byteReceived.Length);
		                    	if (read > 0)
		                    	{
		                        	Console.WriteLine("Messagem Recebida: " + Encoding.ASCII.GetString(byteReceived) + System.Environment.NewLine);
		                    	}
		                    	stream.Flush();
		                }
		                catch (Exception e)
		                {
		                    	Console.WriteLine("Erro: " + e.ToString());
		                }
            		}
            		stream.Close();
		}

	        public void close()
	        {
	        	bluetoothClient.Close();
	            	bluetoothClient.Dispose();
	            	Console.WriteLine("Conexão terminada!");
	        }
	}
}
