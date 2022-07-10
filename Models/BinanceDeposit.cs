using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceApiLibrary
{
    public class BinanceDeposit
    {
        public string Id { get; set; }
        public string Amount { get; set; }
        public string TransactionFee { get; set; }
        public string Coin { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }
        public string TxId { get; set; }
        public string ApplyTime { get; set; }
        public string Network { get; set; }
        public int TransferType { get; set; }
        public string Info { get; set; }
        public int ConfirmNo { get; set; }
        public int WalletType { get; set; }
        public string TxKey { get; set; }
        public string AddressTag { get; set; }

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
            sb.Append(ApplyTime);
            sb.Append($"\nNetwork: ");
            sb.Append(Network);
            sb.Append($"\nTransferType: ");
            sb.Append(TransferType);
            sb.Append($"\nInfo: ");
            sb.Append(Info);
            sb.Append($"\nConfirmNo: ");
            sb.Append(ConfirmNo);
            sb.Append($"\nWalletType: ");
            sb.Append(WalletType);
            sb.Append($"\nTxKey: ");
            sb.Append(TxKey);
            sb.Append($"\nAddressTag: ");
            sb.Append(AddressTag);
            sb.Append("\n");

            return sb.ToString();
        }
    }
}
