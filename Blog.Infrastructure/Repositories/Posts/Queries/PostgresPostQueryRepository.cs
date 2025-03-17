﻿using Blog.Domain.DTOs;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories.Posts.Queries;

public class PostgresPostQueryRepository(PostgresContext context) : IPostRepositories.IPostgresQuery
{
    public async Task<List<PostDocumentDto>> GetLastPostDocument()
    {
        const string query = """
                             SELECT
                                 p.id,
                                 p.status,
                                 p.title,
                                 p.content,
                                 p.created_at,
                                 string_agg(t.title, ',') AS tags
                             FROM
                                 t_post p
                             LEFT JOIN
                                 jsonb_array_elements_text(p.tags) AS tag_id ON TRUE
                             LEFT JOIN
                                 t_tag t ON t.id = tag_id::int
                             WHERE
                                 p.id = (SELECT MAX(id) FROM t_post)
                             GROUP BY
                                 p.id, p.status, p.title, p.content, p.created_at;
                             """;

        var result = await context.Database
            .SqlQueryRaw<PostDocumentDto>(query)
            .ToListAsync();

        return result;
    }
    
    public async Task<List<PostDocumentDto>> GetPostDocumentsByIdRange(int fromId, int toId)
    {
        const string query = """
                             SELECT
                                 p.id,
                                 p.status,
                                 p.title,
                                 p.content,
                                 p.created_at,
                                 string_agg(t.id || '$$' || t.title, ',' ORDER BY t.id) AS tags
                             FROM
                                 public.t_posts p
                                     LEFT JOIN
                                 jsonb_array_elements_text(p.tags) AS tag_id ON TRUE
                                     LEFT JOIN
                                 public.t_tags t ON t.id = tag_id::int
                             WHERE
                                 p.id BETWEEN {0} AND {1}
                             GROUP BY
                                 p.id, p.status, p.title, p.content, p.created_at
                             HAVING
                                 string_agg(t.id || '$$' || t.title, ',' ORDER BY t.id) IS NOT NULL;
                             """;


        var result = await context.Database
            .SqlQueryRaw<PostDocumentDto>(query, fromId, toId)
            .ToListAsync();

        return result;
    }
}