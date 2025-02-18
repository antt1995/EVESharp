﻿using EVESharp.Database.Inventory.Types;
using EVESharp.EVE.Data.Inventory;
using EVESharp.Types.Collections;

namespace EVESharp.EVE.Exceptions;

public class SkillRequired : UserError
{
    public SkillRequired (TypeID skill) : this ((int) skill) { }

    public SkillRequired (int skill) : base (
        "SkillRequired",
        new PyDictionary {["skillName"] = FormatTypeIDAsName (skill)}
    ) { }
}