using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceReserv.IRepository;
using PlaceReserv.Models;
using PlaceReserv.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Dapper;

namespace PlaceReserv.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository reservationRepository;



        public ReservationController(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;

        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return reservation;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            var reservations = await reservationRepository.GetAllReservationsAsync();
            return Ok(reservations);
        }

        [HttpPost]
        [Authorize]

        public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
        {

            await reservationRepository.AddReservationAsync(reservation);
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> UpdateReservation(int id, Reservation reservation)
        {


            if (id != reservation.Id)
            {
                return BadRequest();
            }

            try
            {
                await reservationRepository.UpdateReservationAsync(reservation);
            }
            catch (Exception)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteReservation(int id)
        {

            var reservation = await reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            await reservationRepository.DeleteReservationAsync(id);

            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            var reservation = reservationRepository.GetReservationByIdAsync(id);
            return reservation != null;
        }
    }
}

    