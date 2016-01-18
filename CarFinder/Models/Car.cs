namespace CarFinder.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Car
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(50)]
        public string make { get; set; }

        [StringLength(50)]
        public string model_name { get; set; }

        [StringLength(128)]
        public string model_trim { get; set; }

        [StringLength(10)]
        public string model_year { get; set; }

        [StringLength(50)]
        public string body_style { get; set; }

        [StringLength(50)]
        public string engine_position { get; set; }

        [StringLength(50)]
        public string engine_cc { get; set; }

        [StringLength(50)]
        public string engine_num_cyl { get; set; }

        [StringLength(50)]
        public string engine_type { get; set; }

        [StringLength(50)]
        public string engine_valves_per_cyl { get; set; }

        [StringLength(50)]
        public string engine_power_ps { get; set; }

        [StringLength(50)]
        public string engine_power_rpm { get; set; }

        [StringLength(50)]
        public string engine_torque_nm { get; set; }

        [StringLength(50)]
        public string engine_torque_rpm { get; set; }

        [StringLength(50)]
        public string engine_bore_mm { get; set; }

        [StringLength(50)]
        public string engine_stroke_mm { get; set; }

        [StringLength(50)]
        public string engine_compression { get; set; }

        [StringLength(50)]
        public string engine_fuel { get; set; }

        [StringLength(50)]
        public string top_speed_kph { get; set; }

        [StringLength(50)]
        public string zero_to_100_kph { get; set; }

        [StringLength(50)]
        public string drive_type { get; set; }

        [StringLength(50)]
        public string transmission_type { get; set; }

        [StringLength(50)]
        public string seats { get; set; }

        [StringLength(50)]
        public string doors { get; set; }

        [StringLength(50)]
        public string weight_kg { get; set; }

        [StringLength(50)]
        public string length_mm { get; set; }

        [StringLength(50)]
        public string width_mm { get; set; }

        [StringLength(50)]
        public string height_mm { get; set; }

        [StringLength(50)]
        public string wheelbase { get; set; }

        [StringLength(50)]
        public string lkm_hwy { get; set; }

        [StringLength(50)]
        public string lkm_mixed { get; set; }

        [StringLength(50)]
        public string lkm_city { get; set; }

        [StringLength(50)]
        public string fuel_capacity_l { get; set; }

        [StringLength(50)]
        public string sold_in_us { get; set; }

        [StringLength(50)]
        public string co2 { get; set; }

        [StringLength(50)]
        public string make_display { get; set; }
    }
}
