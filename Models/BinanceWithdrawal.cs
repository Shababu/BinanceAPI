using System.Text;
using BinanceApiLibrary.Deserialization;
using TradingCommonTypes;

namespace BinanceApiLibrary
{
    public class BinanceWithdrawal : IWithdrawal
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string TransactionFee { get; set; }
        public string Coin { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }
        public string TxId { get; set; }
        public DateTime ApplyTime { get; set; }
        public string Network { get; set; }
        public int TransferType { get; set; }
        public string Info { get; set; }
        public int ConfirmNo { get; set; }
        public string TxKey { get; set; }
        public string AddressTag { get; set; }

        internal static BinanceWithdrawal ConvertToWithdrawal(BinanceWithdrawalDeserialization withdrawalRaw)
        {
            BinanceWithdrawal withdrawal = new BinanceWithdrawal()
            {
                Id = withdrawalRaw.Id,
                Amount = Convert.ToDecimal(withdrawalRaw.Amount.Replace('.', ',')),
                TransactionFee = withdrawalRaw.TransactionFee,
                Coin = withdrawalRaw.Coin,
                Status = Convert.ToInt32(withdrawalRaw.Status),
                Address = withdrawalRaw.Address,
                TxId = withdrawalRaw.TxId,
                ApplyTime = DateTime.Parse(withdrawalRaw.ApplyTime),
                Network = withdrawalRaw.Network,
                TransferType = Convert.ToInt32(withdrawalRaw.TransferType),
                Info = withdrawalRaw.Info,
                ConfirmNo = Convert.ToInt32(withdrawalRaw.ConfirmNo),
                TxKey = withdrawalRaw.TxKey,
                AddressTag = withdrawalRaw.AddressTag,
            };

            return withdrawal;
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
            sb.Append(ApplyTime);
            sb.Append($"\nNetwork: ");
            sb.Append(Network);
            sb.Append($"\nTransferType: ");
            sb.Append(TransferType);
            sb.Append($"\nInfo: ");
            sb.Append(Info);
            sb.Append($"\nConfirmNo: ");
            sb.Append(ConfirmNo);
            sb.Append($"\nTxKey: ");
            sb.Append(TxKey);
            sb.Append($"\nAddressTag: ");
            sb.Append(AddressTag);
            sb.Append("\n");

            return sb.ToString();
        }
    }
}
