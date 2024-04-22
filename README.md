le_strings：
  Header Start
    magic:Fixed 0xa84c7f73
    version:UINT16 (Maybe always 1)
    String_Refrence_Number:UINT16
    Le_String_Total_Number:UINT32
  Header End (4+2+2+4)

  StringRefrenceList Start
    loop 0 to String_Refrence_Number:
      ref_item.number:UINT64
      ref_item.offset:UINT64
    loop end
    #Note:if number == 0 or offset == FF FF FF FF, that is invaild.
  StringRefrenceList End

  LeStringCursorList Start
    loop 0 to Le_String_Total_Number:
      le_cursor:UINT64
    loop end
  LeStringCursorList END

  StringList Start
    (See below]
  End

  How To ReadString:
    loop StringRefrenceList:
      Posistion To ref_item.offset
      loop 0 to ref_item.number
        hash:UINT32,fixed,game need this.
        string:Unicode(UTF16),end with 000 00
      loop end

le_strings file = Header StringRefrenceList LeStringCursorList StringList
