﻿using System.Text;
using Newtonsoft.Json;

namespace BinanceApiLibrary.Deserialization
{
    internal class BinanceDepositDeserialization
    {
        public string Id { get; set; }
        public string Amount { get; set; }
        public string TransactionFee { get; set; }
        public string Coin { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string TxId { get; set; }
        public string InsertTime { get; set; }
        public string Network { get; set; }
        public string TransferType { get; set; }
        public string AddressTag { get; set; }

        internal static List<BinanceDepositDeserialization> DeserializeDeposit(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<BinanceDepositDeserialization>>(jsonString);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Id: ");
            sb.Append(Id);
            sb.Append($"\nAmount: ");
            sb.Append(Amount);
            sb.Append($"\nTransactionFee: ");
            sb.Append(TransactionFee);
            sb.Append($"\nCoin: ");
            sb.Append(Coin);
            sb.Append($"\nStatus: ");
            sb.Append(Status);
            sb.Append($"\nAddress: ");
            sb.Append(Address);
            sb.Append($"\nTxId: ");
            sb.Append(TxId);
            sb.Append($"\nApplyTime: ");
            sb.Append(InsertTime);
            sb.Append($"\nNetwork: ");
            sb.Append(Network);
            sb.Append($"\nTransferType: ");
            sb.Append(TransferType);
            sb.Append($"\nAddressTag: ");
            sb.Append(AddressTag);
            sb.Append($"\n");

            return sb.ToString();
        }
    }
}
