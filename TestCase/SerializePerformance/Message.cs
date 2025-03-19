using System;

namespace AutoCSer.TestCase.SerializePerformance
{
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public struct Grade
    {
        public string subject;
        public float value;
    }
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public class Message
    {
        public int age;
        public float weight;
        public string name;
        public LeftArray<Grade> grades;
        public Message()
        {
            age = 0;
            weight = 0;
            name = string.Empty;
            grades.SetEmpty();
        }
    }
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]
    public sealed class JsonMessage : Message
    {
        //Message message = new Message();
        //json(message, count);
        //jsonThreadStatic(message, count);
        //binary(message, count);
        //binarThreadStatic(message, count);

        //JsonMessage jsonMessage = new JsonMessage();
        //binary(jsonMessage, count);
        //binarThreadStatic(jsonMessage, count);
    }
}
