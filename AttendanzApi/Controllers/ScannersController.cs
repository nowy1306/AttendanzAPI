using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AttendanzApi.Interfaces;
using AttendanzApi.Models;
using AttendanzApi.Dtos;
using AttendanzApi.Utils.Generators;
using IronBarCode;

namespace AttendanzApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScannersController : ControllerBase
    {
        private readonly ILogger<GroupsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<ScannerModel> _scanners;

        public ScannersController(
            ILogger<GroupsController> logger,
            IMapper mapper,
            IRepository<ScannerModel> scanners)
        {
            _logger = logger;
            _mapper = mapper;
            _scanners = scanners;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var scanners = _scanners.GetAll();
            var dtos = scanners.Select(scanner => _mapper.Map(scanner, new ScannerDto()));

            return Ok(dtos);
        }

        [HttpGet]
        [Route("{id}/barcode")]
        public IActionResult GetBarcode(long id)
        {
            var scanner = _scanners.GetById(id);
            if (scanner == null)
                return NotFound();

            var barcode = BarcodeWriter.CreateBarcode(scanner.Key, BarcodeWriterEncoding.Code128)
                .ResizeTo(200, 100)
                .AddBarcodeValueTextBelowBarcode();
            var data = barcode.ToPngBinaryData();

            return File(data, "image/png");
        }

        [HttpPost]
        public IActionResult Post([FromBody] ScannerDto dto)
        {
            var scanner = _mapper.Map(dto, new ScannerModel());
            scanner.Key = KeyGenerator.Get128BytesCode();
            _scanners.Insert(scanner);

            return Ok(_mapper.Map(scanner, new ScannerDto()));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Patch(long id, [FromBody] ScannerDto dto)
        {
            var scanner = _scanners.GetById(id);
            if (scanner == null)
                return NotFound();

            _mapper.Map(dto, scanner);
            _scanners.Update(scanner);

            return Ok(_mapper.Map(scanner, new ScannerDto()));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(long id)
        {
            var scanner = _scanners.GetById(id);
            if (scanner == null)
                return NotFound();

            _scanners.Delete(scanner);

            return Ok();
        }

        [HttpPut]
        [Route("{key}/confirmation")]
        public IActionResult Confirm(string key)
        {
            var scanner = _scanners.FirstOrDefault(scanner => scanner.Key == key);
            if (scanner == null)
                return NotFound();

            scanner.IsConfirmed = true;
            _scanners.Update(scanner);

            return Ok();
        }

    }
}
