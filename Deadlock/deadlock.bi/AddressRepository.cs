﻿using deadlock.bl.Repositories.Base;
using deadlock.data.Context;
using deadlock.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace deadlock.bi
{
    public class AddressRepository : Repository<Address>
    {

        public AddressRepository(DeadLockDbContext context) : base(context)
        {

        }

    }
}
