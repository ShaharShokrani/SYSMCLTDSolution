using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SYSMCLTD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SYSMCLTDDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(
            SYSMCLTDDbContext eventsService,
            IMapper mapper,
            ILogger<CustomersController> logger)
        {
            this._context = eventsService ?? throw new ArgumentNullException(nameof(eventsService));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET api/customers
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IQueryable<Customer> query = this._context.Customers.Where(x => !x.IsDeleted);

                query = query.Include(x => x.Addresses).Where(x => !x.IsDeleted);
                query = query.Include(x => x.Contacts).Where(x => !x.IsDeleted);

                List<Customer> customers = await query.ToListAsync();

                if (customers == null)
                {
                    return NoContent();
                }

                IEnumerable<CustomerDTO> result = this._mapper.Map<IEnumerable<CustomerDTO>>(customers);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/customers/1
        [HttpGet("{id}", Name = "GET")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Id is zero");
                }

                IQueryable<Customer> query = this._context.Customers.Where(x => !x.IsDeleted);

                query = query.Include(x => x.Addresses).Where(x => !x.IsDeleted);
                query = query.Include(x => x.Contacts).Where(x => !x.IsDeleted);

                Customer customer = await query.FirstOrDefaultAsync(x => x.Id == id);

                if (customer == null)
                {
                    return NoContent();
                }

                CustomerDTO result = this._mapper.Map<CustomerDTO>(customer);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                //TODO: Log the Exception.
            }
        }

        // POST api/customer
        [HttpPost]
        public async Task<IActionResult> Post(CustomerDTO customerDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //Validate If already exist
                Customer customer = await this._context.Customers.FirstOrDefaultAsync(x => x.CustomerNumber == customerDTO.CustomerNumber);
                if (customer != null)
                {
                    return StatusCode(409);
                }

                customer = this._mapper.Map<Customer>(customerDTO);
                await this._context.Customers.AddAsync(customer);
                await this._context.SaveChangesAsync();

                CustomerDTO result = this._mapper.Map<CustomerDTO>(customer);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                //TODO: Log the Exception.
            }
        }

        // PUT api/customer/
        [HttpPut]
        public async Task<IActionResult> Put(CustomerDTO customerDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                IQueryable<Customer> query = this._context.Customers.Where(x => !x.IsDeleted);

                query = query.Include(x => x.Addresses).Where(x => !x.IsDeleted);
                query = query.Include(x => x.Contacts).Where(x => !x.IsDeleted);

                Customer customer = await query.FirstOrDefaultAsync(x => x.Id == customerDTO.Id);
                if (customer == null)
                {
                    return NoContent();
                }

                customer.Addresses = this._mapper.Map<ICollection<Address>>(customerDTO.Addresses);
                customer.Contacts = this._mapper.Map<ICollection<Contact>>(customerDTO.Contacts);
                customer.CustomerNumber = customerDTO.CustomerNumber;
                customer.Name = customerDTO.Name;

                _context.Entry(customer).State = EntityState.Modified;

                var result = await this._context.SaveChangesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                //TODO: Log the Exception.
            }
        }

        // DELETE api/customers/1
        [HttpDelete("{id}", Name = "DELETE")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                IQueryable<Customer> query = this._context.Customers.Where(x => !x.IsDeleted);

                query = query.Include(x => x.Addresses).Where(x => !x.IsDeleted);
                query = query.Include(x => x.Contacts).Where(x => !x.IsDeleted);

                Customer customer = await query.FirstOrDefaultAsync(x => x.Id == id);
                if (customer == null)
                {
                    return NoContent();
                }

                this._context.Customers.Remove(customer);

                var result = await this._context.SaveChangesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                //TODO: Log the Exception.
            }
        }
    }
}
