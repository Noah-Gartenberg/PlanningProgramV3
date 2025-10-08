using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;

namespace PlanningProgramV3
{
    /**
     * This struct stores version data for the program config class
     * using bytes because it's what I use in the xml schemas, and if I need more than 255 for any of these, I think I'm doing something wrong. 
     * Still not fully sure when to set these, but I'll figure it out.
     */
    public struct VersionData
    {
        //This will specify the most updated data for default version data to be created with
        public static VersionData CurrentVersion = new VersionData(0, 0, 0, 0);

        public byte major;
        public byte minor;
        public byte revision;
        public byte build;

        public VersionData(byte major, byte minor, byte revision, byte build)
        {
            this.major = major;
            this.minor = minor;
            this.revision = revision;
            this.build = build;
        }
    }

    /**
     * This class will store program config information so it can be easily accessed at runtime, while still having it be out of the way. 
     */
    [XmlRootAttribute("ProgramConfig", Namespace="http://tempuri.org/ProgramConfig.xsd", IsNullable=false)]
    [XmlInclude(typeof(VersionData))]
    public class ProgramConfig
    {
        //right now, default set to 0.0.0.0
        //version data should never be set in the constructor - as of right now, should just be set directly-
            //OH AND MAYBE AN ERROR SHOULD BE THROWN IF USER TRIES TO USE A FILE FROM A VERSION THAT'S OLD

        //well, that explains why nothing's getting stored... can't set values that aren't primitive/simple types...

        [XmlElement(ElementName = "SoftwareVersion", Namespace = "http://tempuri.org/ProgramConfig.xsd")]
        public VersionData versionData = VersionData.CurrentVersion;
        [XmlElement(ElementName = "FileStoragePath", Namespace = "http://tempuri.org/ProgramConfig.xsd")]
        public string fileStoragePath;

        //Sets the file storage path to input
        public ProgramConfig(string fileStoragePath)
        {
            this.fileStoragePath = fileStoragePath;
        }

        public void SetFileStoragePath(string fileStoragePath) { this.fileStoragePath = fileStoragePath; }

        //default constructor - don't want to create a folder automatically, unless users say so.
        public ProgramConfig() { }

    }
}
