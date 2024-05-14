﻿using Order.Services.EnumsDTO;

namespace Order.Services.DTO
{
    public class OrderLocationDTO
    {
        protected OrderLocationDTO() { }
        public long Id { get; set; }
        public EPlansDTO Plan { get; set; }
        public DateTime DatePrev { get; set; }
    }
}
