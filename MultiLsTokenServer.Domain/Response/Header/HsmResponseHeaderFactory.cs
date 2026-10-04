namespace MultiLsTokenServer.Domain.Response.Header;

public static class HsmResponseHeaderFactory
{
    public const string UnknownCode = "UE";

    static readonly List<HsmResponseHeader> allHeaders =
    [
        new HsmResponseHeader
        {
            Code = HsmResponseHeader.SuccessCode,
            Identifier = "SUCCESSFUL",
            Description = "The command executed successfully."
        },
        new HsmResponseHeader
        {
            Code = "01",
            Identifier = "DEVICE_FAILURE",
            Description = "There was a hardware or general failure."
        },
        new HsmResponseHeader
        {
            Code = "02",
            Identifier = "FORMAT_ERROR",
            Description = "The format of the command data is incorrect. Can occur if the request contains the incorrect number of parameters. In SM?CI command can occur if the signature length is invalid."
        },
        new HsmResponseHeader
        {
            Code = "03",
            Identifier = "TOKEN_CRC_ERROR",
            Description = "The CRC of the token being verified does not match. Applies to SM?VT only. Can also be used in the ValidationResult field in the SM?VT response."
        },
        new HsmResponseHeader
        {
            Code = "04",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "05",
            Identifier = "KEY_TYPE_ERROR",
            Description = "The KeyType of the specified key register does not meet all of the requirements. Applicable to SM?VC command: cannot vend to VDDK (KT=1) and can only vend to VCDK (KT=3) if TCT is 1 (magnetic card)."
        },
        new HsmResponseHeader
        {
            Code = "06",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "07",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "08",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "09",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "10",
            Identifier = "KEYPAD_ENTRY_CANCELLED",
            Description = "KCED entry/display aborted. Applicable to SM?ML and SM?MG commands only. (HSM manufacturing firmware only)"
        },
        new HsmResponseHeader
        {
            Code = "11",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "12",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "13",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "14",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "15",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "16",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "17",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "18",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "19",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "20",
            Identifier = "CHECKSUM_ERROR",
            Description = "The CRC of the command request is incorrect. This code is only used with a GL!ER response."
        },
        new HsmResponseHeader
        {
            Code = "21",
            Identifier = "INVALID_REQUEST_HEADER",
            Description = "The header of the command request is invalid or the command is not supported. This code is only used with a GL!ER response."
        },
        new HsmResponseHeader
        {
            Code = "22",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "23",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "24",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "25",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "26",
            Identifier = "KMC_KEY_STATE_ERROR",
            Description = "The KEK slot is not in the required state (i.e. LOADING, ACTIVE or PENDING) for the requested command. Applies to SM?KR, SM?KL, SM?KF and SM?VC."
        },
        new HsmResponseHeader
        {
            Code = "27",
            Identifier = "KEK_SLOT_NOT_FOUND_ERROR",
            Description = "A KEK slot with the requested KMCID could not be found (SM?KX), or there are no more available slots  (SM?KC)."
        },
        new HsmResponseHeader
        {
            Code = "28",
            Identifier = "VK_REG_NOT_FOUND_ERROR",
            Description = "During key loading, could not find a VK register with matching attributes that belongs to the specified parent key."
        },
        new HsmResponseHeader
        {
            Code = "29",
            Identifier = "KEY_CHANGE_NOT_ALLOWED",
            Description = "One of the rules for a key change has not been met. Applies to SM?VK only."
        },
        new HsmResponseHeader
        {
            Code = "30",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "31",
            Identifier = "INSUFFICIENT_CREDIT_BALANCE",
            Description = "The credit limit associated with the specified  VK has been depleted. Refer to ULM and CLM key attributes. Applies to SM?VC only."
        },
        new HsmResponseHeader
        {
            Code = "32",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "33",
            Identifier = "INVALID_SECURITY_MODULE_ID",
            Description = "ModuleID does not match that provided in the certificate. Applies to SM?CI command only."
        },
        new HsmResponseHeader
        {
            Code = "34",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "35",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "36",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "37",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "38",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "39",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "40",
            Identifier = "KEY_PARAM_ERROR",
            Description = "A parameter associated with a VK is outside of its required range. For example EA, TI, DKGA, KT, KRN,  Class, SubClass."
        },
        new HsmResponseHeader
        {
            Code = "41",
            Identifier = "KEY_EXPIRED_ERROR",
            Description = "TID > KEN, or KMC key expired. Applies to SM?VC, SM?VM and SM?KC only. Can also be used in the ValidationResult field in the SM?VT response."
        },
        new HsmResponseHeader
        {
            Code = "42",
            Identifier = "KEY_ISSUE_EXPIRED_ERROR",
            Description = "Current module time > IUT. Applies to SM?VC, SM?VM, SM?VK. Can also be used in the ValidationResult field of the SM?VT response."
        },
        new HsmResponseHeader
        {
            Code = "43",
            Identifier = "KEY_NOT_YET_ACTIVE",
            Description = "Current module time < ACT. Applies to SM?KC and SM?VK only."
        },
        new HsmResponseHeader
        {
            Code = "44",
            Identifier = "TID_OUT_OF_WINDOW",
            Description = "Current module time is outside of TokenID-to-RTC window. Applies to SM?VC and SM?VM only."
        },
        new HsmResponseHeader
        {
            Code = "45",
            Identifier = "LOG_RANGE_ERROR",
            Description = "No more entries available to read or log is empty. Applicable to SM?QL."
        },
        new HsmResponseHeader
        {
            Code = "46",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "47",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "48",
            Identifier = "SUBCLASS_NOT_ALLOWED",
            Description = "Specified SubClass not included in the set of allowed SubClass values for this VK. SM?VC only."
        },
        new HsmResponseHeader
        {
            Code = "49",
            Identifier = "DF_FORMAT_ERROR",
            Description = "Error in dfconcat formatting of a request, including an invalid CRC if the dfconcat string has one  (see 5.7.1 of STS 600-4-2). Applies to SM?KC, SM?KR and SM?KL commands. Can also occur when formatting a response if the stored data is corrupt or invalid, although this is not expected behaviour."
        },
        new HsmResponseHeader
        {
            Code = "50",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "51",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "52",
            Identifier = "KEYPAD_ENTRY_ERROR",
            Description = "An error occurred during KCED entry/display. Applicable to SM?ML and SM?MG commands in HSM manufacturing firmware only."
        },
        new HsmResponseHeader
        {
            Code = "53",
            Identifier = "KEYPAD_NOT_FOUND",
            Description = "A KCED is not connected to the CSP port. Applicable to SM?ML and SM?MG commands in HSM manufacturing firmware only."
        },
        new HsmResponseHeader
        {
            Code = "54",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "55",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "56",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "57",
            Identifier = "TAMPERED_STATE",
            Description = "A tamper event has occurred. This code is only used with a GL!ER response. SM?QL will provide insight. Module must be returned to manufacturer."
        },
        new HsmResponseHeader
        {
            Code = "58",
            Identifier = "APP_ERROR_STATE",
            Description = "Internal error. Power cycling may resolve, otherwise permanent. This code is only used with a GL!ER response."
        },
        new HsmResponseHeader
        {
            Code = "59",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "60",
            Identifier = "PTVD_NFIELDS",
            Description = "Error in PTVD formatted command request. Too many or too few fields, or a partial field was encountered."
        },
        new HsmResponseHeader
        {
            Code = "61",
            Identifier = "PTVD_FIELDTYPE",
            Description = "Error in PTVD formatted command request. A different type to what was expected (for a field in that position of the request)."
        },
        new HsmResponseHeader
        {
            Code = "62",
            Identifier = "PTVD_ENCODING",
            Description = "Error in PTVD formatted command request. The encoding of a field is invalid for that field’s type."
        },
        new HsmResponseHeader
        {
            Code = "63",
            Identifier = "PTVD_RANGE",
            Description = "Error in PTVD formatted command request. Field decoded  ok but the value is outside of the permitted range."
        },
        new HsmResponseHeader
        {
            Code = "64",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "65",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "66",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "67",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "68",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "69",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "70",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "71",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "72",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "73",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "74",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "75",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "76",
            Identifier = "INVALID_PAN_ERROR",
            Description = "PrimaryAccountNumber (PAN) is not 18 digits or has an incorrect check digit. Applicable to SM?MK and SM?VK/VC/VM/VT."
        },
        new HsmResponseHeader
        {
            Code = "77",
            Identifier = "AUTHENTICATION_ERROR",
            Description = "Failed to verify expected mac (applies to SM?KL)."
        },
        new HsmResponseHeader
        {
            Code = "78",
            Identifier = "CHECK_DIGIT_ERROR",
            Description = "The key check digits (KCV) for the key components entered via the KCED do not match the expected value. Indicates that one or more components have been entered incorrectly. Applicable to the SM?ML command in HSM manufacturing firmware only."
        },
        new HsmResponseHeader
        {
            Code = "79",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "80",
            Identifier = "DDTK_CREDIT_ERROR",
            Description = "Used in the [ValidationResult] field in the SM?VT response if using a default key with a Class 0 token."
        },
        new HsmResponseHeader
        {
            Code = "81",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "82",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "83",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "84",
            Identifier = "Reserved for future use",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "85",
            Identifier = "INVALID_NUM_TOKENS",
            Description = "The number of tokens requested in the SM?VK command is not valid for the chosen EncryptionAlgorithm (EA)."
        },
        new HsmResponseHeader
        {
            Code = "86",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "87",
            Identifier = "INVALID_INSTRUCTION",
            Description = "Signed instruction has invalid parameters. Applicable to SM?CI."
        },
        new HsmResponseHeader
        {
            Code = "88",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "89",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "90",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "91",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "92",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "93",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "94",
            Identifier = "ATTRIBUTE_ERROR",
            Description = "Applies to SM?KL only. A mandatory key attribute (in card format) has not been provided, or an attribute does not meet the parameter constraints for this attribute."
        },
        new HsmResponseHeader
        {
            Code = "95",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "96",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "97",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "98",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "99",
            Identifier = "Proprietary",
            Description = ""
        },
        new HsmResponseHeader
        {
            Code = "EE",
            Identifier = "STS_EXTENDED_ERROR",
            Description = "Extended error (see 5.3 of STS600-8-6). For the SM?KC and SM?KR commands the extended error message texts correspond to the error messages specified in clauses 11 and 13 of STS600-4-2."
        }
    ];

    public static readonly HsmResponseHeader UnknownCodeHeader = new()
    {
        Code = UnknownCode,
        Identifier = "UNKNOWN_ERROR_CODE",
        Description = "Unespected Error Occurred."
    };

    public static HsmResponseHeader ParameterOutOfRangeHeader(string? description = null) => new()
    {
        Code = UnknownCode,
        Identifier = "PARAMETER_OUT_OF_RANGE",
        Description = description ?? "Invalid value in parameter detected."
    };

    public static HsmResponseHeader KeyExpiredHeader(string? description = null) => new()
    {
        Code = UnknownCode,
        Identifier = "VENDING_KEY_EXPIRED",
        Description = description ?? "Vending Key Expired."
    };

    public static HsmResponseHeader ParameterValidationError(string? description = null) => new()
    {
        Code = UnknownCode,
        Identifier = "PARAMETER_VALIDATION_ERROR",
        Description = description ?? "Invalid value in parameter detected."
    };

    public static HsmResponseHeader NoConnectionHeader(string? description = null) => new()
    {
        Code = UnknownCode,
        Identifier = "NO_HSM_CONNECTION",
        Description = description ?? "No Connection with the HSM."
    };

    public static HsmResponseHeader NoHsmResponseHeader(string? description = null) => new()
    {
        Code = UnknownCode,
        Identifier = "NO_HSM_RESPONSE",
        Description = description ?? "No Response from the HSM."
    };

    public static HsmResponseHeader ResponseGenerationExceptionHeader(string? description = null) => new()
    {
        Code = UnknownCode,
        Identifier = "RESPONSE_GENERATION_ERROR",
        Description = description ?? "Error in the Response Generation Process."
    };

    static readonly Dictionary<string, HsmResponseHeader> headers = allHeaders.ToDictionary(x => x.Code);

    public static HsmResponseHeader CreateHeaderFromCode(string code) => headers.GetValueOrDefault(code) ?? UnknownCodeHeader;
}
