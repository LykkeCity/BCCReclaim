using Core.Bitcoin;
using Core.Repositories.Settings;
using Core.Settings;
using Microsoft.AspNetCore.Mvc;
using NBitcoin;
using NBitcoin.RPC;
using QBitNinja.Client;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BCCReclaimApi.Controllers
{
    [Route("api/[controller]")]
    public class BroadcastController : Controller
    {
        BaseSettings settings;

        public BroadcastController(BaseSettings _settings) : base()
        {
            settings = _settings;
        }

        [HttpGet("Broadcast")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Broadcast([FromQuery]string transaction)
        {
            try
            {
                RpcConnectionParams connectionParams = new RpcConnectionParams(settings);
                RPCClient client = new RPCClient(new NetworkCredential(connectionParams.UserName, connectionParams.Password), connectionParams.IpAddress, connectionParams.Network);

                await client.SendRawTransactionAsync(new Transaction(transaction));
            }
            catch(Exception exp)
            {
                throw exp;
            }
            return Ok();
        }
    }
}
