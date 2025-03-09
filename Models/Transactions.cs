﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        public long? transaction_id { get; set; }
        public int? bill_id { get; set; }
        public decimal? total_money { get; set; }
        public DateTime? payment_date { get; set; }
        public string? status_name { get; set; }
        public string? payment_method { get; set; }
        public string? bank_code { get; set; }
        public string? message {  get; set; }
        public string? decription { get; set; }

        [ForeignKey(nameof(bill_id))]
        public virtual Bill? Bill { get; set; }

    }
}
