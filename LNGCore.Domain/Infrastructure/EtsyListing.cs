using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.Infrastructure
{

    public class EtsyListings
    {
        public int count { get; set; }
        public Result[] results { get; set; }
        public Params _params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }
    }

    public class Params
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public object page { get; set; }
        public string shop_id { get; set; }
        public object keywords { get; set; }
        public string sort_on { get; set; }
        public string sort_order { get; set; }
        public object min_price { get; set; }
        public object max_price { get; set; }
        public object color { get; set; }
        public int color_accuracy { get; set; }
        public object tags { get; set; }
        public object category { get; set; }
        public string translate_keywords { get; set; }
        public int include_private { get; set; }
    }

    public class Pagination
    {
        public int effective_limit { get; set; }
        public int effective_offset { get; set; }
        public object next_offset { get; set; }
        public int effective_page { get; set; }
        public object next_page { get; set; }
    }

    public class Result
    {
        public int listing_id { get; set; }
        public string state { get; set; }
        public int user_id { get; set; }
        public int? category_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int creation_tsz { get; set; }
        public int ending_tsz { get; set; }
        public int original_creation_tsz { get; set; }
        public int last_modified_tsz { get; set; }
        public string price { get; set; }
        public string currency_code { get; set; }
        public int quantity { get; set; }
        public object[] sku { get; set; }
        public string[] tags { get; set; }
        public string[] category_path { get; set; }
        public int[] category_path_ids { get; set; }
        public string[] materials { get; set; }
        public int shop_section_id { get; set; }
        public int? featured_rank { get; set; }
        public int state_tsz { get; set; }
        public string url { get; set; }
        public int views { get; set; }
        public int num_favorers { get; set; }
        public long? shipping_template_id { get; set; }
        public int processing_min { get; set; }
        public int processing_max { get; set; }
        public string who_made { get; set; }
        public string is_supply { get; set; }
        public string when_made { get; set; }
        public string item_weight { get; set; }
        public string item_weight_unit { get; set; }
        public string item_length { get; set; }
        public string item_width { get; set; }
        public string item_height { get; set; }
        public string item_dimensions_unit { get; set; }
        public bool is_private { get; set; }
        public string recipient { get; set; }
        public string occasion { get; set; }
        public string[] style { get; set; }
        public bool non_taxable { get; set; }
        public bool is_customizable { get; set; }
        public bool is_digital { get; set; }
        public string file_data { get; set; }
        public bool should_auto_renew { get; set; }
        public string language { get; set; }
        public bool has_variations { get; set; }
        public int taxonomy_id { get; set; }
        public string[] taxonomy_path { get; set; }
        public bool used_manufacturer { get; set; }
        public Image[] Images { get; set; }
    }

    public class Image
    {
        public int listing_image_id { get; set; }
        public string hex_code { get; set; }
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public int hue { get; set; }
        public int saturation { get; set; }
        public int brightness { get; set; }
        public bool? is_black_and_white { get; set; }
        public int creation_tsz { get; set; }
        public int listing_id { get; set; }
        public int rank { get; set; }
        public string url_75x75 { get; set; }
        public string url_170x135 { get; set; }
        public string url_570xN { get; set; }
        public string url_fullxfull { get; set; }
        public int full_height { get; set; }
        public int full_width { get; set; }
    }

}
