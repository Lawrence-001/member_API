using AutoMapper;
using MemberTestAPI.Dtos;
using MemberTestAPI.Models;
using MemberTestAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemberTestAPI.Controllers
{
    [Route("api/member-registrations")]
    [ApiController]
    public class MemberRegistrationController : ControllerBase
    {
        private readonly IMemberRegistrationRepository _memberRegistrationRepository;
        private readonly IMapper _mapper;

        public MemberRegistrationController(IMemberRegistrationRepository memberRegistrationRepository, IMapper mapper)
        {
            _memberRegistrationRepository = memberRegistrationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegistrations()
        {
            return Ok(await _memberRegistrationRepository.GetAllRegistrations());
        }

        [HttpGet("{registrationId:int}")]
        public async Task<IActionResult> GetRegistrationById(int registrationId)
        {
            var reg = await _memberRegistrationRepository.GetRegistrationById(registrationId);
            if (reg != null)
            {
                var result = _mapper.Map<MemberRegistrationDto>(reg);
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddMemberRegistration([FromBody] CreateMemberRegistrationDto createMemberRegistrationDto)
        {
            if (createMemberRegistrationDto != null)
            {
                var memberReg = _mapper.Map<MemberRegistration>(createMemberRegistrationDto);
                var result = await _memberRegistrationRepository.CreateRegistration(memberReg);
                return CreatedAtAction(nameof(GetRegistrationById), new { registrationId = result.RegistrationId }, result);
            }
            //return NotFound();
            return BadRequest();
        }

        [HttpPut("{registrationId:int}")]
        public async Task<IActionResult> UpdateRegistration([FromRoute] int registrationId, [FromBody] CreateMemberRegistrationDto updateMemberRegistrationDto)
        {
            var reg_member = await _memberRegistrationRepository.GetRegistrationById(registrationId);
            if (reg_member != null)
            {
                //var member_to_update = _mapper.Map<MemberRegistration>(updateRegistrationDto);

                _mapper.Map(updateMemberRegistrationDto, reg_member); //Merges new data into existing object
                //await _memberRepository.UpdateRegistration(registrationId, member_to_update);
                await _memberRegistrationRepository.UpdateRegistration(registrationId, reg_member);
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("{registrationId:int}")]
        public async Task<IActionResult> DeleteRegistration(int registrationId)
        {
            var reg_to_delete = await _memberRegistrationRepository.GetRegistrationById(registrationId);
            if (reg_to_delete != null)
            {
                await _memberRegistrationRepository.DeleteRegistration(registrationId);
                return NoContent();
            }
            return NotFound();
        }
    }
}
