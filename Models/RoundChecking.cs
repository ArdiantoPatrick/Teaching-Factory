using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjekTeFa.Models
{
    public class RoundChecking
    {
        [Key]
        public int id_roundchecking { get; set; }
        public DateTime tanggal { get; set; }
        public TimeSpan waktu { get; set; }
        public string vin { get; set; }
        public string nama_sa { get; set; }
        public string deskripsi_service { get; set; }
        public string perkiraan_biaya { get; set; }
        public string declines_sparetire { get; set; }
        public string declines_glovebox { get; set; }
        public string e_horn_operation { get; set; }
        public string e_lights { get; set; }
        public string e_washer_operation { get; set; }
        public string e_winshield { get; set; }
        public string e_fuel_tank_cap { get; set; }
        public string? i_combination_meter { get; set; }
        public string? i_cabin_air_filter { get; set; }
        public string? i_parking_brake { get; set; }
        public string? i_floor { get; set; }
        public string? uh_air_filter { get; set; }
        public string? uh_battery_condition { get; set; }
        public string? uh_battery_state { get; set; }
        public string? uh_cooling_system { get; set; }
        public string? uh_hoses { get; set; }
        public string? uh_drive_belts { get; set; }
        public string? uh_radiator_core { get; set; }
        public string? f_windshield_washer { get; set; }
        public string? f_clutch_reservoir { get; set; }
        public string? f_coolant { get; set; }
        public string? f_power_steering { get; set; }
        public string? f_brake_reservoir { get; set; }
        public string? f_transmission { get; set; }
        public string? f_differential { get; set; }
        public string? f_transfercase { get; set; }
        public string? uv_propeller { get; set; }
        public string? uv_drive_shaft { get; set; }
        public string? uv_axle_hub { get; set; }
        public string? uv_steering { get; set; }
        public string? uv_suspension { get; set; }
        public string? uv_fluid_leaks { get; set; }
        public string? uv_exhaust_system { get; set; }
        public string? uv_fuel_connections { get; set; }
        public string? t_tread_depth { get; set; }
        public string? t_tire_damage { get; set; }
        public string? t_rims { get; set; }
        public string? t_rotated { get; set; }
        public string? b_brake_lining { get; set; }
        public string? b_hoses { get; set; }
        public string? b_discs { get; set; }
        public string? komentar { get; set; }
        public string? status { get; set; }

        [ForeignKey("Booking")]
        public int? id_booking { get; set; }
        public Booking? Booking { get; set; }

    }
}
