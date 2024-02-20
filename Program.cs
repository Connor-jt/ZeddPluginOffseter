using System.Xml;

namespace ZeddPluginOffseter{
    internal class Program{
		static XmlDocument xdoc;
        static void Main(string[] args){
            Console.WriteLine("Hello, World!");

			string source_path = "C:\\Users\\Joe bingle\\Downloads\\Halo5\\(scnr)scenario.xml";


            xdoc = new();
            xdoc.Load(source_path);
            var root_node = xdoc.SelectSingleNode("plugin");

			recurse_traverse(root_node, 0);

			xdoc.Save(source_path+"_offsets.xml");
        }


		static long recurse_traverse(XmlNode search_node, long current_offset){
			long beginning_offset = current_offset;
			foreach (XmlNode child in search_node.ChildNodes){

                XmlAttribute attr = xdoc.CreateAttribute("offset");
				attr.Value = current_offset.ToString();
				child.Attributes.Append(attr);

                long offset = group_lengths_dict[child.Name];
                current_offset += offset;

                switch (child.Name){
					case "_field_struct":
					case "_field_array": // instead of having a count, there are 'Length' amount of children, thanks i guess
                        long struct_size = recurse_traverse(child, current_offset);
                        XmlAttribute struct_size_attr = xdoc.CreateAttribute("size");
                        struct_size_attr.Value = struct_size.ToString();
                        child.Attributes.Append(struct_size_attr);

						current_offset += struct_size;
                        break;
					case "_field_block_64":
						long block_size = recurse_traverse(child, 0);
                        XmlAttribute block_size_attr = xdoc.CreateAttribute("size");
                        block_size_attr.Value = block_size.ToString();
                        child.Attributes.Append(block_size_attr);
                        break;

					case "_field_pad":
                    case "_field_skip":
                        current_offset += Convert.ToInt32(child.Attributes["length"].Value);
                        break;
            }
            }
			return current_offset - beginning_offset; // convert offset to size of struct
		}







		public static Dictionary<string, long> group_lengths_dict = new() {
			{ "_field_string", 32 },   
			{ "_field_long_string", 256 },  
			{ "_field_string_id", 4 },    
			{ "unk0", 4 },    
			{ "_field_char_integer", 1 },    
			{ "_field_short_integer", 2 },    
			{ "_field_long_integer", 4 },    
			{ "_field_int64_integer", 8 },    
			{ "_field_angle", 4 },    
			{ "_field_tag", 4 },    
			{ "_field_char_enum", 1 },    
			{ "_field_short_enum", 2 },    
			{ "_field_long_enum", 4 },    
			{ "_field_long_flags", 4 },    
			{ "_field_word_flags", 2 },    
			{ "_field_byte_flags", 1 },    
			{ "_field_point_2d", 4 },   
			{ "_field_rectangle_2d", 4 },   
			{ "_field_rgb_color", 4 },   
			{ "_field_argb_color ", 4 },   
			{ "_field_real", 4 },   
			{ "_field_real_fraction", 4 },   
			{ "_field_real_point_2d", 8 },   
			{ "_field_real_point_3d", 12 },  
			{ "_field_real_vector_2d  ", 8 },   
			{ "_field_real_vector_3d", 12 },  
			{ "_field_real_quaternion", 16 },  
			{ "_field_real_euler_angles_2d", 8 },   
			{ "_field_real_euler_angles_3d", 12 },  
			{ "_field_real_plane_2d", 12 },  
			{ "_field_real_plane_3d", 16 },  
			{ "_field_real_rgb_color", 12 },  
			{ "_field_real_argb_color", 16 },  
			{ "_field_real_hsv_color", 4 },   
			{ "_field_real_ahsv_color", 4 },   
			{ "_field_short_integer_bounds", 4 },   
			{ "_field_angle_bounds", 8 },   
			{ "_field_real_bounds", 8 },   
			{ "_field_fraction_bounds", 8 },
			{ "unk1", 4 },   
			{ "unk2", 4 },   
			{ "_field_long_block_flags", 4 },   
			{ "_field_word_block_flags", 4 },   
			{ "_field_byte_block_flags", 4 },   
			{ "_field_char_block_index", 1 },   
			{ "_field_custom_char_block_index", 1 },   
			{ "_field_short_block_index", 2 },   
			{ "_field_custom_short_block_index", 2 },   
			{ "_field_long_block_index", 4 },   
			{ "_field_custom_long_block_index", 4 },   
			{ "unk3", 4 },   
			{ "unk4", 4 },   
			{ "_field_pad", 0 },   
			{ "_field_skip", 0 },   
			{ "_field_explanation", 0 },   
			{ "_field_custom", 0 },   
			{ "_field_struct", 0 },   
			{ "_field_array", 0 },  
			{ "unk5", 4 },   
            { "unk6", 0 },   
			{ "_field_byte_integer", 1 },   
			{ "_field_word_integer", 2 },   
			{ "_field_dword_integer", 4 },   
			{ "_field_qword_integer", 8 },   
			{ "_field_block_64", 28 },  
			{ "_field_tag_reference_64", 32 }, 
			{ "_field_data_64", 28 },  
			{ "_field_pageable_resource_64", 16 },  
			{ "unk7", 4 },
            { "unk8", 4 },
        };
    }
}