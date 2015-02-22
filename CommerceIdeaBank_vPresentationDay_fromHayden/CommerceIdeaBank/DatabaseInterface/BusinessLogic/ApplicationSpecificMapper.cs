using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.DatabaseInterface.BusinessLogic
{
    //Class to provide local mapping configurations for CommerceIdeaBank
    public class ApplicationSpecificMapper
    {
        //Constructors
        public ApplicationSpecificMapper()
        {

        }

        //Func Desc: Function to make mapping easy
        //    Input: A source object to be converted into an object of type dest_type.
        //   Output: An object of type destination type, or null.
        public object Map(object source, Type dest_type)
        {
            //Input checks
            if (source == null) { return null; }

            Mapper.CreateMap(source.GetType(), dest_type);

            return (object)Mapper.Map(source, source.GetType(), dest_type);
        }


        //Func Desc: (OVERLOAD) Function to make mapping easy
        //    Input: A source object of type source_type
        //           to be converted into an object of type dest_type.
        //   Output: An object of type destination type, or null.
        public object Map(object source, Type source_type, Type dest_type)
        {
            //Input checks
            if (source == null) { return null; }

            Mapper.CreateMap(source_type, dest_type);

            return (object)Mapper.Map(source, source_type, dest_type);
        }


        //Func Desc: Used to map all objects in a list to an object of type destination type.
        //    Input: List<object> of objects to perform mapping on, dest_type indicating the objects
        //           to which each object in the list will be converted.
        //   Output: List<object> of objects after mapping, or null
        public IEnumerable<object> MapAll(IEnumerable<object> source_list, Type dest_type)
        {
            //Input checks
            if (source_list == null) { return null; }
            if (source_list.ToList().Count == 0) { return null; }

            //Create local var to store object type
            Type source_type = source_list.ElementAt(0).GetType();

            //Local declarations
            List<object> dest_list = new List<object>();
            IEnumerable<object> dest_enum;

            //Perform mapping for each domain model
            foreach (var dom_model in source_list)
            {
                //Map domain model and then add it to destination list
                dest_list.Add( Map(dom_model, source_type, dest_type) );
            }

            dest_enum = dest_list.Cast<object>().AsEnumerable();

            //Return list of mapped objects
            return dest_enum;
        }
    }
}