namespace StoreManageAPI.Helpers.SendEmail
{
    public class FormatHTML
    {
        public static string FormatConfirmEmailHTML(string? code, string? title, int time)
        {
            var html = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>{title}</title>\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            margin: 0;\r\n            padding: 20px;\r\n        }}\r\n        .email-container {{\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);\r\n        }}\r\n        .header {{\r\n            text-align: center;\r\n            padding-bottom: 20px;\r\n        }}\r\n        .header h1 {{\r\n            color: #333333;\r\n        }}\r\n        .verification-code {{\r\n            font-size: 24px;\r\n            font-weight: bold;\r\n            color: #4CAF50;\r\n            text-align: center;\r\n            margin: 20px 0;\r\n            padding: 10px;\r\n            border: 2px dashed #4CAF50;\r\n            border-radius: 8px;\r\n            background-color: #f9f9f9;\r\n        }}\r\n        .content {{\r\n            font-size: 16px;\r\n            color: #555555;\r\n            text-align: center;\r\n        }}\r\n        .footer {{\r\n            text-align: center;\r\n            margin-top: 20px;\r\n            font-size: 14px;\r\n            color: #888888;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"email-container\">\r\n        <div class=\"header\">\r\n            <h1>{title}</h1>\r\n        </div>\r\n        <div class=\"content\">\r\n            <h2>Xin Chào,</h2>\r\n            <p>{title}. Vui lòng sử dụng đoạn mã sau để xác minh địa chỉ email của bạn:</p>\r\n            <div class=\"verification-code\">\r\n                {code}\r\n            </div>\r\n            <p>Mã này có giá trị trong {time} phút.</p>\r\n            <p>Nếu bạn không yêu cầu điều này, vui lòng bỏ qua email này.</p>\r\n        </div>\r\n        <div class=\"footer\">\r\n            <p>&copy; Nguyễn Khánh Sơn.</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>";

            return html;
        }

    }
}
