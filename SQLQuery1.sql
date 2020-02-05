select b.* from Solution a, Applications b, Solution_Application c 
where a.Id = c.SolutionId  and b.Id = c.ApplicationId  and a.Id = '1'