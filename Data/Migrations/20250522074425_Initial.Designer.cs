﻿// <auto-generated />
using Logrus.Smith.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logrus.Smith.Data.Migrations
{
    [DbContext(typeof(AgentDbContext))]
    [Migration("20250522074425_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Logrus.Smith.Data.Entities.Agent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("InitialState")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("initial_state");

                    b.Property<int>("InputTokensLimit")
                        .HasColumnType("integer")
                        .HasColumnName("input_tokens_limit");

                    b.Property<int>("InputTokensUsed")
                        .HasColumnType("integer")
                        .HasColumnName("input_tokens_used");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("OutputTokensLimit")
                        .HasColumnType("integer")
                        .HasColumnName("output_tokens_limit");

                    b.Property<int>("OutputTokensUsed")
                        .HasColumnType("integer")
                        .HasColumnName("output_tokens_used");

                    b.Property<string>("StateJson")
                        .HasColumnType("jsonb")
                        .HasColumnName("state_json");

                    b.Property<Instant>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_agents");

                    b.ToTable("agents", (string)null);
                });

            modelBuilder.Entity("Logrus.Smith.Data.Entities.AgentOutcome", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AgentId")
                        .HasColumnType("bigint")
                        .HasColumnName("agent_id");

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("data");

                    b.Property<decimal>("Score")
                        .HasColumnType("numeric")
                        .HasColumnName("score");

                    b.HasKey("Id")
                        .HasName("pk_agent_outcomes");

                    b.HasIndex("AgentId")
                        .HasDatabaseName("ix_agent_outcomes_agent_id");

                    b.ToTable("agent_outcomes", (string)null);
                });

            modelBuilder.Entity("Logrus.Smith.Data.Entities.AgentOutcome", b =>
                {
                    b.HasOne("Logrus.Smith.Data.Entities.Agent", "Agent")
                        .WithMany("Outcomes")
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_agent_outcomes_agents_agent_id");

                    b.Navigation("Agent");
                });

            modelBuilder.Entity("Logrus.Smith.Data.Entities.Agent", b =>
                {
                    b.Navigation("Outcomes");
                });
#pragma warning restore 612, 618
        }
    }
}
