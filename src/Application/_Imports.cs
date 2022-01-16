// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

global using MediatR;
global using Microsoft.Extensions.Localization;
global using AutoMapper;
global using AutoMapper.QueryableExtensions;
global using FluentValidation;
global using Microsoft.EntityFrameworkCore;
global using System.Data;
global using System.Linq.Dynamic.Core;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Primitives;
global using Microsoft.Extensions.Caching.Memory;
global using CleanArchitecture.Razor.Domain.Enums;
global using CleanArchitecture.Razor.Domain.Entities;
global using CleanArchitecture.Razor.Domain.Events;
global using CleanArchitecture.Razor.Application.Common.Mappings;
global using CleanArchitecture.Razor.Application.Common.Models;
global using CleanArchitecture.Razor.Application.Common.Extensions;
global using CleanArchitecture.Razor.Application.Common.Interfaces;
global using CleanArchitecture.Razor.Application.Common.Interfaces.Caching;
global using CleanArchitecture.Razor.Domain.Entities.Audit;
global using CleanArchitecture.Razor.Domain.Entities.Log;
global using CleanArchitecture.Razor.Application.Common.Specification;
