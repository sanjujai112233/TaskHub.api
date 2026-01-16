using System;
using AutoMapper;
using TaskHub.api.Dtos;
using TaskHub.api.Models;

namespace TaskHub.api.Profiles;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskItem, TaskReadDto>();
        CreateMap<TaskCreatedDto, TaskItem>();
    }

}
