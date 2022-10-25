using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Product.Queries.GetProductById
{
    public class GetProductByIdQueryRequest: IRequest<GetProductByIdQueryResponse>
    {
        public string Id { get; set; }
    }

}
