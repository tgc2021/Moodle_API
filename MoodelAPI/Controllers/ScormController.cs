using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MoodelAPI.Controllers
{
    [EnableCorsAttribute(origins: "*", headers: "*", methods: "*")]
    public class ScormController : ApiController
    {
        [HttpOptions]
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type"); // Specify allowed headers here
            return response;
        }
        public class RsTypeQna
        {
            public int IdRsTypeQna { get; set; }  // id_rs_type_qna (Primary Key, Auto Increment)
            public int IdAssessmentLog { get; set; }  // id_assessment_log
            public int IdUser { get; set; }  // id_user
            public int IdOrganization { get; set; }  // id_organization
            public int IdAssessmentSheet { get; set; }  // id_assessment_sheet
            public int IdAssessment { get; set; }  // id_assessment
            public int AttemptNumber { get; set; }  // attempt_number
            public int TotalQuestion { get; set; }  // total_question
            public int RightAnswerCount { get; set; }  // right_answer_count
            public int WrongAnswerCount { get; set; }  // wrong_answer_count
            public double ResultInPercentage { get; set; }  // result_in_percentage
            public string Status { get; set; }  // status (varchar)
            public DateTime UpdatedDateTime { get; set; }  // updated_date_time
            public string moodle_assessment_status { get; set; }  // updated_date_time
        }



        [HttpPost]
        [Route("api/scorm/insertQna")]
        public IHttpActionResult InsertQna([FromBody] RsTypeQna qna)
        {
            if (qna == null)
            {
                return BadRequest("Invalid input data.");
            }

            string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"INSERT INTO tbl_rs_type_qna 
                         (id_assessment_log, id_user, id_organization, id_assessment_sheet, id_assessment, 
                          attempt_number, total_question, right_answer_count, wrong_answer_count, 
                          result_in_percentage, status, updated_date_time,moodle_assessment_status) 
                          VALUES 
                          (@IdAssessmentLog, @IdUser, @IdOrganization, @IdAssessmentSheet, @IdAssessment, 
                           @AttemptNumber, @TotalQuestion, @RightAnswerCount, @WrongAnswerCount, 
                           @ResultInPercentage, @Status, @UpdatedDateTime,@moodle_assessment_status)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Assuming qna.IdAssessmentLog should be passed from the body or auto-generated
                        cmd.Parameters.AddWithValue("@IdAssessmentLog", 500);  // Replace 500 with dynamic or passed value
                        cmd.Parameters.AddWithValue("@IdUser", qna.IdUser);
                        cmd.Parameters.AddWithValue("@IdOrganization", qna.IdOrganization);
                        cmd.Parameters.AddWithValue("@IdAssessmentSheet", qna.IdAssessmentSheet);
                        cmd.Parameters.AddWithValue("@IdAssessment", qna.IdAssessment);
                        cmd.Parameters.AddWithValue("@AttemptNumber", qna.AttemptNumber);
                        cmd.Parameters.AddWithValue("@TotalQuestion", qna.TotalQuestion);
                        cmd.Parameters.AddWithValue("@RightAnswerCount", qna.RightAnswerCount);
                        cmd.Parameters.AddWithValue("@WrongAnswerCount", qna.WrongAnswerCount);
                        cmd.Parameters.AddWithValue("@ResultInPercentage", qna.ResultInPercentage);
                        cmd.Parameters.AddWithValue("@Status", "A");  // Replace with dynamic status if needed
                        cmd.Parameters.AddWithValue("@UpdatedDateTime", DateTime.Now);  // Fixed DateTime usage
                        cmd.Parameters.AddWithValue("@moodle_assessment_status", qna.moodle_assessment_status);  // Fixed DateTime usage

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return Ok("Data inserted successfully.");
                        }
                        else
                        {
                            return BadRequest("Insert operation failed.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }

    }
}
